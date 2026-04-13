using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.BUS;

public class OrderService : IOrderService
{
    private const string GenericSystemErrorMessage = "Không thể xử lý hóa đơn do lỗi hệ thống.";
    private const string ConcurrencyErrorMessage = "Dữ liệu đã bị thay đổi bởi nhân viên khác. Vui lòng tải lại!";
    private const string InvoiceEntityName = "Invoice";
    private const string TakeAwayTableName = "Mang đi";
    private static readonly JsonSerializerOptions AuditJsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    private readonly IActivityLogWriter _activityLogWriter;

    public OrderService(IActivityLogWriter? activityLogWriter = null)
    {
        _activityLogWriter = activityLogWriter ?? new ActivityLogService();
    }

    public BanActionResultDTO AddItemToOrder(int orderId, int productId, short quantity, byte[]? expectedRowVersion = null)
    {
        return AddItemsToOrder(
            orderId,
            new[]
            {
                new BanHangThemMonDTO
                {
                    MonID = productId,
                    SoLuong = quantity
                }
            },
            successMessage: "Thêm món vào hóa đơn thành công.",
            expectedRowVersion: expectedRowVersion);
    }

    public OperationResult AddItemsByTableAtomic(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem, int? khachHangId = null)
    {
        if (banId <= 0)
        {
            return ToOperationResult(BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn trước khi gọi món."));
        }

        if (dsMonThem == null)
        {
            throw new ArgumentNullException(nameof(dsMonThem));
        }

        var dsMonHopLe = TongHopMonThemHopLe(dsMonThem);
        if (dsMonHopLe.Count == 0)
        {
            return ToOperationResult(BusMessageCatalog.CreateActionResult(false, "Danh sách món gọi không hợp lệ."));
        }

        using var strategyContext = new CaPheDbContext();
        var strategy = strategyContext.Database.CreateExecutionStrategy();
        var ketQua = OperationResult.Failure(GenericSystemErrorMessage);

        strategy.Execute(() =>
        {
            using var context = new CaPheDbContext();
            using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            try
            {
                var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
                if (ban == null)
                {
                    ketQua = ToOperationResult(BusMessageCatalog.CreateActionResult(false, "Không tìm thấy bàn để gọi món."));
                    transaction.Rollback();
                    return;
                }

                var hoaDon = context.HoaDon
                    .Include(x => x.HoaDon_ChiTiet)
                    .Where(x => x.BanID == banId
                        && x.TrangThai != (int)HoaDonTrangThai.Closed
                        && x.TrangThai != (int)HoaDonTrangThai.Cancelled)
                    .OrderByDescending(x => x.NgayLap)
                    .ThenByDescending(x => x.ID)
                    .FirstOrDefault();

                if (hoaDon == null)
                {
                    if (!TryLayNhanVienDangNhapHopLe(context, out var nhanVienId, out var thongBaoNhanVien))
                    {
                        ketQua = ToOperationResult(BusMessageCatalog.CreateActionResult(false, thongBaoNhanVien));
                        transaction.Rollback();
                        return;
                    }

                    var khachHangIdHopLe = khachHangId.HasValue && khachHangId.Value > 0
                        ? khachHangId.Value
                        : (int?)null;

                    var tenKhachHang = "Khách lẻ";
                    if (khachHangIdHopLe.HasValue)
                    {
                        var khachHang = context.KhachHang
                            .AsNoTracking()
                            .Where(x => x.ID == khachHangIdHopLe.Value)
                            .Select(x => new
                            {
                                x.ID,
                                x.HoVaTen
                            })
                            .FirstOrDefault();

                        if (khachHang == null)
                        {
                            ketQua = ToOperationResult(BusMessageCatalog.CreateActionResult(false, "Khách hàng đã chọn không tồn tại hoặc đã bị xóa."));
                            transaction.Rollback();
                            return;
                        }

                        khachHangIdHopLe = khachHang.ID;
                        tenKhachHang = string.IsNullOrWhiteSpace(khachHang.HoVaTen)
                            ? "Khách lẻ"
                            : khachHang.HoVaTen;
                    }

                    hoaDon = new dtaHoadon
                    {
                        BanID = banId,
                        NhanVienID = nhanVienId,
                        KhachHangID = khachHangIdHopLe,
                        CustomerName = tenKhachHang,
                        NgayLap = DateTime.Now,
                        TrangThai = (int)HoaDonTrangThai.Open,
                        TongTien = 0m,
                        GhiChuHoaDon = string.Empty
                    };

                    context.HoaDon.Add(hoaDon);
                }

                var pendingAuditLogsInTransaction = new List<PendingActivityLog>();
                var ketQuaThemMon = ProcessAddItemsToDraftInvoice(
                    context,
                    hoaDon,
                    dsMonHopLe,
                    pendingAuditLogsInTransaction,
                    successMessage: "Gọi món thành công.");

                if (!ketQuaThemMon.ThanhCong)
                {
                    ketQua = ToOperationResult(ketQuaThemMon);
                    transaction.Rollback();
                    return;
                }

                AppendPendingAuditLogsToContext(context, pendingAuditLogsInTransaction);
                context.SaveChanges();
                transaction.Commit();

                ketQua = ToOperationResult(ketQuaThemMon);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                AppLogger.Error(ex, $"AddItemsByTableAtomic failed. BanID={banId}.", nameof(OrderService));
                throw;
            }
        });

        return ketQua;
    }

    public BanActionResultDTO AddItemsToOrder(
        int orderId,
        IEnumerable<BanHangThemMonDTO> dsMonThem,
        string successMessage = "Gọi món thành công.",
        byte[]? expectedRowVersion = null)
    {
        if (orderId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi thêm món.");
        }

        var dsMonHopLe = TongHopMonThemHopLe(dsMonThem);
        if (dsMonHopLe.Count == 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Danh sách món gọi không hợp lệ.");
        }

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon, pendingAuditLogs) =>
            ProcessAddItemsToDraftInvoice(context, hoaDon, dsMonHopLe, pendingAuditLogs, successMessage));
    }

    private static BanActionResultDTO ProcessAddItemsToDraftInvoice(
        CaPheDbContext context,
        dtaHoadon hoaDon,
        IReadOnlyCollection<BanHangThemMonDTO> dsMonHopLe,
        ICollection<PendingActivityLog> pendingAuditLogs,
        string successMessage)
    {
        if (!HoaDonStateMachine.IsOpen(hoaDon.TrangThai))
        {
            return BusMessageCatalog.CreateActionResult(false, "Chỉ thêm món cho hóa đơn Open.");
        }

        var snapshotCu = TaoSnapshotHoaDon(hoaDon);

        var dsMonId = dsMonHopLe
            .Select(x => x.MonID)
            .Distinct()
            .ToArray();

        var dsMonDb = context.Mon
            .Where(x => dsMonId.Contains(x.ID))
            .ToDictionary(x => x.ID);

        if (dsMonDb.Count != dsMonId.Length)
        {
            return BusMessageCatalog.CreateActionResult(false, "Có món không tồn tại trong hệ thống.");
        }

        if (dsMonDb.Values.Any(x => x.TrangThai != 1))
        {
            return BusMessageCatalog.CreateActionResult(false, "Có món đang ngừng bán, vui lòng tải lại danh sách món.");
        }

        var dsCongThuc = QueryCongThucMonRows(context, dsMonId);
        var loiCongThuc = KiemTraDayDuCongThuc(dsMonId, dsCongThuc);
        if (loiCongThuc != null)
        {
            return loiCongThuc;
        }

        var dsNhuCauNguyenLieu = TinhNhuCauNguyenLieu(dsMonHopLe, dsCongThuc);
        var dsNguyenLieuId = dsNhuCauNguyenLieu
            .Select(x => x.NguyenLieuID)
            .Distinct()
            .ToArray();

        var dsNguyenLieuDb = context.NguyenLieu
            .Where(x => dsNguyenLieuId.Contains(x.ID))
            .ToDictionary(x => x.ID);

        if (dsNguyenLieuDb.Count != dsNguyenLieuId.Length)
        {
            return BusMessageCatalog.CreateActionResult(false, "Công thức món tham chiếu nguyên liệu không tồn tại.");
        }

        var loiTonKho = KiemTraTonKho(dsNhuCauNguyenLieu, dsNguyenLieuDb);
        if (loiTonKho != null)
        {
            return loiTonKho;
        }

        ThemHoacCapNhatChiTietHoaDon(hoaDon, dsMonHopLe, dsMonDb);
        TinhLaiTongTienHoaDon(hoaDon);
        TruTonKhoNguyenLieu(context, hoaDon.ID, hoaDon.BanID, hoaDon.NhanVienID, dsNhuCauNguyenLieu, dsNguyenLieuDb);
        CapNhatTrangThaiBan(context, hoaDon.BanID, trangThaiMoi: 1);
        ThemPendingAuditLog(
            pendingAuditLogs,
            AuditActionConstants.AddItem,
            hoaDon.ID,
            TaoMoTaThemMonVaoHoaDon(hoaDon.ID, dsMonHopLe, dsMonDb),
            snapshotCu,
            TaoSnapshotHoaDon(hoaDon));

        return BusMessageCatalog.CreateActionResult(true, successMessage);
    }

    private static bool TryLayNhanVienDangNhapHopLe(
        CaPheDbContext context,
        out int nhanVienId,
        out string thongBao)
    {
        nhanVienId = 0;
        thongBao = "Không xác định được nhân viên đang thao tác. Vui lòng đăng nhập lại.";

        var nguoiDungDangNhap = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        if (nguoiDungDangNhap == null || nguoiDungDangNhap.NhanVienId <= 0)
        {
            return false;
        }

        var nhanVienTonTai = context.NhanVien
            .AsNoTracking()
            .Any(x => x.ID == nguoiDungDangNhap.NhanVienId);
        if (!nhanVienTonTai)
        {
            thongBao = "Tài khoản đăng nhập không liên kết nhân viên hợp lệ. Vui lòng liên hệ quản trị viên.";
            return false;
        }

        nhanVienId = nguoiDungDangNhap.NhanVienId;
        thongBao = string.Empty;
        return true;
    }

    private static void AppendPendingAuditLogsToContext(CaPheDbContext context, IEnumerable<PendingActivityLog> pendingLogs)
    {
        foreach (var log in pendingLogs)
        {
            context.AuditLog.Add(new dtaAuditLog
            {
                Action = string.IsNullOrWhiteSpace(log.Action) ? AuditActionConstants.UpdateInvoice : log.Action,
                EntityName = string.IsNullOrWhiteSpace(log.EntityName) ? InvoiceEntityName : log.EntityName,
                EntityId = string.IsNullOrWhiteSpace(log.EntityId) ? "-" : log.EntityId,
                OldValue = SerializeAuditPayload(log.OldValue),
                NewValue = SerializeAuditPayload(log.NewValue),
                PerformedBy = string.IsNullOrWhiteSpace(log.PerformedBy) ? "system" : log.PerformedBy!,
                CreatedAt = DateTime.Now
            });
        }
    }

    private static string? SerializeAuditPayload(object? payload)
    {
        if (payload == null)
        {
            return null;
        }

        try
        {
            return JsonSerializer.Serialize(payload, AuditJsonOptions);
        }
        catch
        {
            return null;
        }
    }

    private static OperationResult ToOperationResult(BanActionResultDTO result)
    {
        return new OperationResult
        {
            ThanhCong = result.ThanhCong,
            MaThongBao = result.MaThongBao,
            ThongBao = result.ThongBao
        };
    }

    public BanActionResultDTO RemoveItemFromOrder(int orderId, int productId, short quantity, byte[]? expectedRowVersion = null)
    {
        if (orderId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi thêm món.");
        }

        if (productId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn món hợp lệ.");
        }

        if (quantity <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Số lượng món phải lớn hơn 0.");
        }

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon, pendingAuditLogs) =>
        {
            if (!HoaDonStateMachine.IsOpen(hoaDon.TrangThai))
            {
                return BusMessageCatalog.CreateActionResult(false, "Chỉ xóa món cho hóa đơn Open.");
            }

            var snapshotCu = TaoSnapshotHoaDon(hoaDon);
            var tenMon = context.Mon
                .AsNoTracking()
                .Where(x => x.ID == productId)
                .Select(x => x.TenMon)
                .FirstOrDefault() ?? $"Món {productId}";

            var dsChiTietMon = hoaDon.HoaDon_ChiTiet
                .Where(x => x.MonID == productId && x.GhiChu == null)
                .OrderByDescending(x => x.ID)
                .ToList();

            if (dsChiTietMon.Count == 0)
            {
                return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy món đã chọn.");
            }

            var tongSoLuongMon = dsChiTietMon.Sum(x => x.SoLuongBan);
            if (tongSoLuongMon < quantity)
            {
                return BusMessageCatalog.CreateActionResult(false, "Số lượng món cần xóa vượt quá số lượng hiện có trong hóa đơn.");
            }

            var dsCongThuc = QueryCongThucMonRows(context, new[] { productId });
            if (dsCongThuc.Count == 0)
            {
                return BusMessageCatalog.CreateActionResult(false, "Món chưa có công thức chế biến, không thể gọi món.");
            }

            var dsNhuCauHoanTon = TinhNhuCauHoanTonChoMon(productId, quantity, dsCongThuc);
            var dsNguyenLieuId = dsNhuCauHoanTon
                .Select(x => x.NguyenLieuID)
                .Distinct()
                .ToArray();

            var dsNguyenLieuDb = context.NguyenLieu
                .Where(x => dsNguyenLieuId.Contains(x.ID))
                .ToDictionary(x => x.ID);

            if (dsNguyenLieuDb.Count != dsNguyenLieuId.Length)
            {
                return BusMessageCatalog.CreateActionResult(false, "Công thức món tham chiếu nguyên liệu không tồn tại.");
            }

            var soLuongCanXoa = quantity;
            foreach (var chiTiet in dsChiTietMon)
            {
                if (soLuongCanXoa <= 0)
                {
                    break;
                }

                var soLuongXoaTaiDong = Math.Min(soLuongCanXoa, chiTiet.SoLuongBan);
                if (soLuongXoaTaiDong >= chiTiet.SoLuongBan)
                {
                    context.HoaDon_ChiTiet.Remove(chiTiet);
                }
                else
                {
                    chiTiet.SoLuongBan = (short)Math.Clamp(chiTiet.SoLuongBan - soLuongXoaTaiDong, 1, short.MaxValue);
                    CapNhatThanhTienChiTiet(chiTiet);
                }

                soLuongCanXoa -= soLuongXoaTaiDong;
            }

            TinhLaiTongTienHoaDon(hoaDon);

            HoanTonKhoNguyenLieu(
                context,
                hoaDon.NhanVienID,
                dsNhuCauHoanTon,
                dsNguyenLieuDb,
                ghiChuNhapKho: TaoGhiChuNhapKhoTuXoaMon(hoaDon.ID, productId));

            CapNhatTrangThaiBan(context, hoaDon.BanID, trangThaiMoi: 1);
            ThemPendingAuditLog(
                pendingAuditLogs,
                AuditActions.RemoveItem,
                hoaDon.ID,
                $"Đã xóa {quantity} x {tenMon} khỏi hóa đơn HD{hoaDon.ID:D5}.",
                snapshotCu,
                TaoSnapshotHoaDon(hoaDon));

            return BusMessageCatalog.CreateActionResult(true, "Đã xóa món khỏi hóa đơn và hoàn tồn kho thành công.");
        });
    }

    public BanActionResultDTO UpdateItemQuantity(int orderId, int productId, short quantity, byte[]? expectedRowVersion = null)
    {
        if (orderId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi thêm món.");
        }

        if (productId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn món hợp lệ.");
        }

        if (quantity <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Số lượng món phải lớn hơn 0.");
        }

        using var context = new CaPheDbContext();
        var soLuongHienTai = context.HoaDon_ChiTiet
            .AsNoTracking()
            .Where(x => x.HoaDonID == orderId && x.MonID == productId && x.GhiChu == null)
            .Sum(x => (int?)x.SoLuongBan) ?? 0;

        if (soLuongHienTai <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy món đã chọn.");
        }

        if (quantity == soLuongHienTai)
        {
            return BusMessageCatalog.CreateActionResult(true, "Số lượng món không thay đổi.");
        }

        if (quantity > soLuongHienTai)
        {
            var soLuongThem = quantity - soLuongHienTai;
            return AddItemToOrder(orderId, productId, (short)soLuongThem, expectedRowVersion);
        }

        var soLuongXoa = soLuongHienTai - quantity;
        return RemoveItemFromOrder(orderId, productId, (short)soLuongXoa, expectedRowVersion);
    }

    public BanActionResultDTO ReplaceItemInOrder(
        int orderId,
        int currentProductId,
        int newProductId,
        short quantity,
        byte[]? expectedRowVersion = null)
    {
        if (orderId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi đổi món.");
        }

        if (currentProductId <= 0 || newProductId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn món hợp lệ.");
        }

        if (quantity <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Số lượng món phải lớn hơn 0.");
        }

        if (currentProductId == newProductId)
        {
            return BusMessageCatalog.CreateActionResult(true, "Món không thay đổi.");
        }

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon, pendingAuditLogs) =>
        {
            if (!HoaDonStateMachine.IsOpen(hoaDon.TrangThai))
            {
                return BusMessageCatalog.CreateActionResult(false, "Chỉ sửa món cho hóa đơn Open.");
            }

            var snapshotCu = TaoSnapshotHoaDon(hoaDon);

            var dsChiTietMonHienTai = hoaDon.HoaDon_ChiTiet
                .Where(x => x.MonID == currentProductId && x.GhiChu == null)
                .OrderByDescending(x => x.ID)
                .ToList();

            if (dsChiTietMonHienTai.Count == 0)
            {
                return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy món cần đổi trong hóa đơn.");
            }

            var tongSoLuongMonHienTai = dsChiTietMonHienTai.Sum(x => (int)x.SoLuongBan);
            if (tongSoLuongMonHienTai < quantity)
            {
                return BusMessageCatalog.CreateActionResult(false, "Số lượng đổi món vượt quá số lượng hiện có.");
            }

            var monMoi = context.Mon.FirstOrDefault(x => x.ID == newProductId);
            if (monMoi == null)
            {
                return BusMessageCatalog.CreateActionResult(false, "Món mới không tồn tại trong hệ thống.");
            }

            if (monMoi.TrangThai != 1)
            {
                return BusMessageCatalog.CreateActionResult(false, "Món mới đang ngừng bán, vui lòng chọn món khác.");
            }

            var tenMonCu = context.Mon
                .AsNoTracking()
                .Where(x => x.ID == currentProductId)
                .Select(x => x.TenMon)
                .FirstOrDefault() ?? $"Món {currentProductId}";

            var dsMonCanKiemTraCongThuc = new[] { currentProductId, newProductId };
            var dsCongThuc = QueryCongThucMonRows(context, dsMonCanKiemTraCongThuc);
            var loiCongThuc = KiemTraDayDuCongThuc(dsMonCanKiemTraCongThuc, dsCongThuc);
            if (loiCongThuc != null)
            {
                return loiCongThuc;
            }

            var dsNhuCauHoanTon = TinhNhuCauHoanTonChoMon(currentProductId, quantity, dsCongThuc);
            var dsNhuCauMonMoi = TinhNhuCauNguyenLieu(
                new[]
                {
                    new BanHangThemMonDTO
                    {
                        MonID = newProductId,
                        SoLuong = quantity
                    }
                },
                dsCongThuc);

            var dsBienDongTonKho = TinhBienDongTonKhoDoiMon(dsNhuCauHoanTon, dsNhuCauMonMoi);
            var dsNguyenLieuId = dsBienDongTonKho
                .Select(x => x.NguyenLieuID)
                .Distinct()
                .ToArray();

            var dsNguyenLieuDb = context.NguyenLieu
                .Where(x => dsNguyenLieuId.Contains(x.ID))
                .ToDictionary(x => x.ID);

            if (dsNguyenLieuDb.Count != dsNguyenLieuId.Length)
            {
                return BusMessageCatalog.CreateActionResult(false, "Công thức món tham chiếu nguyên liệu không tồn tại.");
            }

            var loiTonKho = KiemTraTonKhoTheoBienDong(dsBienDongTonKho, dsNguyenLieuDb);
            if (loiTonKho != null)
            {
                return loiTonKho;
            }

            var soLuongCanGiamMonCu = quantity;
            foreach (var chiTiet in dsChiTietMonHienTai)
            {
                if (soLuongCanGiamMonCu <= 0)
                {
                    break;
                }

                var soLuongGiamTaiDong = Math.Min(soLuongCanGiamMonCu, chiTiet.SoLuongBan);
                if (soLuongGiamTaiDong >= chiTiet.SoLuongBan)
                {
                    context.HoaDon_ChiTiet.Remove(chiTiet);
                }
                else
                {
                    chiTiet.SoLuongBan = (short)Math.Clamp(chiTiet.SoLuongBan - soLuongGiamTaiDong, 1, short.MaxValue);
                    CapNhatThanhTienChiTiet(chiTiet);
                }

                soLuongCanGiamMonCu -= soLuongGiamTaiDong;
            }

            ThemHoacCapNhatChiTietHoaDon(
                hoaDon,
                new[]
                {
                    new BanHangThemMonDTO
                    {
                        MonID = newProductId,
                        SoLuong = quantity
                    }
                },
                new Dictionary<int, dtaMon>
                {
                    [newProductId] = monMoi
                });

            ApDungBienDongTonKhoDoiMon(context, hoaDon.ID, hoaDon.NhanVienID, currentProductId, newProductId, dsBienDongTonKho, dsNguyenLieuDb);

            TinhLaiTongTienHoaDon(hoaDon);
            CapNhatTrangThaiBan(context, hoaDon.BanID, trangThaiMoi: 1);
            ThemPendingAuditLog(
                pendingAuditLogs,
                AuditActions.ReplaceItem,
                hoaDon.ID,
                $"Đã đổi {quantity} x {tenMonCu} thành {monMoi.TenMon} trong hóa đơn HD{hoaDon.ID:D5}.",
                snapshotCu,
                TaoSnapshotHoaDon(hoaDon));

            return BusMessageCatalog.CreateActionResult(true, "Đổi món thành công.");
        });
    }

    public BanActionResultDTO CancelOrder(int orderId, byte[]? expectedRowVersion = null)
    {
        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap()?.TenDangNhap ?? "system";
        return VoidInvoice(orderId, "Hủy hóa đơn", nguoiDung, expectedRowVersion);
    }

    public BanActionResultDTO VoidInvoice(int orderId, string reason, string user, byte[]? expectedRowVersion = null)
    {
        if (orderId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn cần hủy.");
        }

        var lyDoHuy = string.IsNullOrWhiteSpace(reason) ? "Không có lý do" : reason.Trim();
        var nguoiHuy = string.IsNullOrWhiteSpace(user) ? "system" : user.Trim();

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon, pendingAuditLogs) =>
        {
            if (!HoaDonStateMachine.IsOpen(hoaDon.TrangThai))
            {
                return BusMessageCatalog.CreateActionResult(false, "Chỉ được Void hóa đơn Open.");
            }

            var snapshotCu = TaoSnapshotHoaDon(hoaDon);

            var dsChiTietHopLe = hoaDon.HoaDon_ChiTiet
                .Where(x => x.GhiChu == null && x.SoLuongBan > 0)
                .ToList();

            if (dsChiTietHopLe.Count > 0)
            {
                var dsMonTongHop = dsChiTietHopLe
                    .GroupBy(x => x.MonID)
                    .Select(g => new BanHangThemMonDTO
                    {
                        MonID = g.Key,
                        SoLuong = (short)Math.Clamp(g.Sum(x => (int)x.SoLuongBan), 1, short.MaxValue)
                    })
                    .ToList();

                var dsMonId = dsMonTongHop
                    .Select(x => x.MonID)
                    .Distinct()
                    .ToArray();

                var dsCongThuc = QueryCongThucMonRows(context, dsMonId);
                var dsMonCoCongThuc = dsCongThuc
                    .Select(x => x.MonID)
                    .Distinct()
                    .ToHashSet();

                var dsMonCoTheHoanTon = dsMonTongHop
                    .Where(x => dsMonCoCongThuc.Contains(x.MonID))
                    .ToList();

                if (dsMonCoTheHoanTon.Count > 0)
                {
                    var dsNhuCauHoanTon = TinhNhuCauNguyenLieu(dsMonCoTheHoanTon, dsCongThuc);
                    var dsNguyenLieuId = dsNhuCauHoanTon
                        .Select(x => x.NguyenLieuID)
                        .Distinct()
                        .ToArray();

                    var dsNguyenLieuDb = context.NguyenLieu
                        .Where(x => dsNguyenLieuId.Contains(x.ID))
                        .ToDictionary(x => x.ID);

                    HoanTonKhoNguyenLieu(
                        context,
                        hoaDon.NhanVienID,
                        dsNhuCauHoanTon,
                        dsNguyenLieuDb,
                        ghiChuNhapKho: TaoGhiChuNhapKhoTuHuyHoaDon(hoaDon.ID));
                }
            }

            hoaDon.TrangThai = (int)HoaDonTrangThai.Voided;
            hoaDon.GhiChuHoaDon = $"[Hủy {DateTime.Now:dd/MM/yyyy HH:mm}] {lyDoHuy}. {hoaDon.GhiChuHoaDon}".Trim();

            CapNhatTrangThaiBan(context, hoaDon.BanID, 0);
            TinhLaiTongTienHoaDon(hoaDon);
            ThemPendingAuditLog(
                pendingAuditLogs,
                AuditActionConstants.VoidInvoice,
                hoaDon.ID,
                $"Đã hủy hóa đơn HD{hoaDon.ID:D5}. Lý do: {lyDoHuy}.",
                snapshotCu,
                new
                {
                    Reason = lyDoHuy,
                    CancelledBy = nguoiHuy,
                    Snapshot = TaoSnapshotHoaDon(hoaDon)
                },
                performedBy: nguoiHuy);

            return BusMessageCatalog.CreateActionResult(true, "Hủy hóa đơn thành công.");
        });
    }

    public BanActionResultDTO CancelInvoice(int orderId, string reason, string user, byte[]? expectedRowVersion = null)
    {
        return VoidInvoice(orderId, reason, user, expectedRowVersion);
    }

    public BanActionResultDTO Checkout(int orderId, byte[]? expectedRowVersion = null)
    {
        if (orderId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi thu tiền.");
        }

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon, pendingAuditLogs) =>
        {
            if (!HoaDonStateMachine.IsOpen(hoaDon.TrangThai))
            {
                return BusMessageCatalog.CreateActionResult(false, "Hóa đơn không ở trạng thái Open (chờ thanh toán).");
            }

            var snapshotCu = TaoSnapshotHoaDon(hoaDon);
            TinhLaiTongTienHoaDon(hoaDon);

            if (hoaDon.TongTien <= 0)
            {
                return BusMessageCatalog.CreateActionResult(false, "Hóa đơn chưa có món, không thể thu tiền.");
            }

            if (!HoaDonStateMachine.CanTransition(hoaDon.TrangThai, (int)HoaDonTrangThai.Paid))
            {
                return BusMessageCatalog.CreateActionResult(false, "Không thể chuyển trạng thái hóa đơn theo luồng POS.");
            }

            hoaDon.TrangThai = (int)HoaDonTrangThai.Paid;
            ThemPendingAuditLog(
                pendingAuditLogs,
                AuditActions.PayInvoice,
                hoaDon.ID,
                $"Đã thanh toán hóa đơn HD{hoaDon.ID:D5} với tổng tiền {hoaDon.TongTien:N0}.",
                snapshotCu,
                TaoSnapshotHoaDon(hoaDon));

            return BusMessageCatalog.CreateActionResult(true, $"Thanh toán hóa đơn HD{hoaDon.ID:D5} thành công.");
        });
    }

    private static void ThemHoacCapNhatChiTietHoaDon(
        dtaHoadon hoaDon,
        IEnumerable<BanHangThemMonDTO> dsMonHopLe,
        IReadOnlyDictionary<int, dtaMon> dsMonDb)
    {
        var chiTietTheoMonGia = hoaDon.HoaDon_ChiTiet
            .Where(x => x.GhiChu == null)
            .GroupBy(x => (x.MonID, x.DonGiaBan))
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var monThem in dsMonHopLe)
        {
            var mon = dsMonDb[monThem.MonID];
            var key = (monThem.MonID, mon.DonGia);

            if (!chiTietTheoMonGia.TryGetValue(key, out var chiTiet))
            {
                chiTiet = new dtHoaDon_ChiTiet
                {
                    HoaDonID = hoaDon.ID,
                    MonID = monThem.MonID,
                    SoLuongBan = monThem.SoLuong,
                    DonGiaBan = mon.DonGia,
                    ThanhTien = LamTronTien(monThem.SoLuong * mon.DonGia),
                    GhiChu = null
                };

                hoaDon.HoaDon_ChiTiet.Add(chiTiet);
                chiTietTheoMonGia[key] = chiTiet;
                continue;
            }

            chiTiet.SoLuongBan = (short)Math.Clamp(chiTiet.SoLuongBan + monThem.SoLuong, 1, short.MaxValue);
            CapNhatThanhTienChiTiet(chiTiet);
        }
    }

    private BanActionResultDTO ExecuteTransactional(
        int orderId,
        byte[]? expectedRowVersion,
        Func<CaPheDbContext, dtaHoadon, IList<PendingActivityLog>, BanActionResultDTO> processOrder)
    {
        try
        {
            using var strategyContext = new CaPheDbContext();
            var strategy = strategyContext.Database.CreateExecutionStrategy();

            var ketQua = BusMessageCatalog.CreateActionResult(false, GenericSystemErrorMessage);
            List<PendingActivityLog> pendingAuditLogs = new();

            strategy.ExecuteAsync(async () =>
            {
                await using var context = new CaPheDbContext();
                await using var transaction = await context.Database
                    .BeginTransactionAsync(System.Data.IsolationLevel.Serializable)
                    .ConfigureAwait(false);

                var pendingAuditLogsInTransaction = new List<PendingActivityLog>();

                var hoaDon = await context.HoaDon
                    .Include(x => x.HoaDon_ChiTiet)
                    .FirstOrDefaultAsync(x => x.ID == orderId)
                    .ConfigureAwait(false);

                if (hoaDon == null)
                {
                    ketQua = BusMessageCatalog.CreateActionResult(false, "Không tìm thấy hóa đơn cần xử lý.");
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    return;
                }

                if (expectedRowVersion is { Length: > 0 })
                {
                    context.Entry(hoaDon)
                        .Property(x => x.RowVersion)
                        .OriginalValue = expectedRowVersion;
                }

                ketQua = processOrder(context, hoaDon, pendingAuditLogsInTransaction);
                if (!ketQua.ThanhCong)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    return;
                }

                try
                {
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);
                    pendingAuditLogs = pendingAuditLogsInTransaction;
                }
                catch (DbUpdateConcurrencyException)
                {
                    ketQua = BusMessageCatalog.CreateActionResult(false, ConcurrencyErrorMessage);
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    await TaiLaiHoaDonMoiNhat(orderId).ConfigureAwait(false);
                }
            }).ConfigureAwait(false).GetAwaiter().GetResult();

            if (ketQua.ThanhCong)
            {
                GhiPendingAuditLogs(pendingAuditLogs);
            }

            return BusMessageCatalog.NormalizeActionResult(ketQua);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BusMessageCatalog.CreateActionResult(false, ConcurrencyErrorMessage);
        }
        catch
        {
            return BusMessageCatalog.CreateActionResult(false, GenericSystemErrorMessage);
        }
    }

    private static void TinhLaiTongTienHoaDon(dtaHoadon hoaDon)
    {
        foreach (var chiTiet in hoaDon.HoaDon_ChiTiet.Where(x => x.SoLuongBan > 0))
        {
            CapNhatThanhTienChiTiet(chiTiet);
        }

        hoaDon.TongTien = LamTronTien(hoaDon.HoaDon_ChiTiet
            .Where(x => x.SoLuongBan > 0)
            .Sum(x => x.ThanhTien));
    }

    private static void CapNhatThanhTienChiTiet(dtHoaDon_ChiTiet chiTiet)
    {
        chiTiet.ThanhTien = LamTronTien(chiTiet.SoLuongBan * chiTiet.DonGiaBan);
    }

    private static decimal LamTronTien(decimal giaTri)
    {
        return decimal.Round(giaTri, 2, MidpointRounding.AwayFromZero);
    }

    private static HoaDonSnapshot TaoSnapshotHoaDon(dtaHoadon hoaDon)
    {
        return new HoaDonSnapshot
        {
            HoaDonId = hoaDon.ID,
            TrangThai = hoaDon.TrangThai,
            TongTien = hoaDon.TongTien,
            TongMon = hoaDon.HoaDon_ChiTiet
                .Where(x => x.SoLuongBan > 0)
                .Sum(x => (int)x.SoLuongBan),
            ChiTiet = hoaDon.HoaDon_ChiTiet
                .OrderBy(x => x.MonID)
                .ThenBy(x => x.ID)
                .Select(x => new HoaDonSnapshotItem
                {
                    MonID = x.MonID,
                    SoLuong = x.SoLuongBan,
                    DonGia = x.DonGiaBan,
                    ThanhTien = x.ThanhTien
                })
                .ToList()
        };
    }

    private void GhiPendingAuditLogs(IEnumerable<PendingActivityLog> pendingLogs)
    {
        foreach (var log in pendingLogs)
        {
            _activityLogWriter.Log(
                log.UserId,
                log.Action,
                log.EntityName,
                log.EntityId,
                log.Description,
                log.OldValue,
                log.NewValue,
                log.PerformedBy);
        }
    }

    private static void ThemPendingAuditLog(
        ICollection<PendingActivityLog> pendingLogs,
        string action,
        int hoaDonId,
        string description,
        object? oldValue,
        object? newValue,
        string? performedBy = null)
    {
        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        pendingLogs.Add(new PendingActivityLog
        {
            UserId = nguoiDung?.UserId,
            Action = action,
            EntityName = InvoiceEntityName,
            EntityId = hoaDonId.ToString(CultureInfo.InvariantCulture),
            Description = description,
            OldValue = oldValue,
            NewValue = newValue,
            PerformedBy = string.IsNullOrWhiteSpace(performedBy)
                ? nguoiDung?.TenDangNhap
                : performedBy
        });
    }

    private static string TaoMoTaThemMonVaoHoaDon(
        int hoaDonId,
        IEnumerable<BanHangThemMonDTO> dsMonThem,
        IReadOnlyDictionary<int, dtaMon> dsMonDb)
    {
        var dsMoTaMon = new List<string>();

        foreach (var monThem in dsMonThem)
        {
            if (!dsMonDb.TryGetValue(monThem.MonID, out var mon))
            {
                dsMoTaMon.Add($"{monThem.SoLuong} x Món {monThem.MonID}");
                continue;
            }

            dsMoTaMon.Add($"{monThem.SoLuong} x {mon.TenMon}");
        }

        return $"Đã thêm {string.Join(", ", dsMoTaMon)} vào hóa đơn HD{hoaDonId:D5}.";
    }

    private static async Task TaiLaiHoaDonMoiNhat(int orderId)
    {
        await using var reloadContext = new CaPheDbContext();
        _ = await reloadContext.HoaDon
            .AsNoTracking()
            .Include(x => x.HoaDon_ChiTiet)
            .FirstOrDefaultAsync(x => x.ID == orderId)
            .ConfigureAwait(false);
    }

    private static List<BanHangThemMonDTO> TongHopMonThemHopLe(IEnumerable<BanHangThemMonDTO> dsMonThem)
    {
        return dsMonThem
            .Where(x => x.MonID > 0 && x.SoLuong > 0)
            .GroupBy(x => x.MonID)
            .Select(g => new BanHangThemMonDTO
            {
                MonID = g.Key,
                SoLuong = (short)Math.Clamp(g.Sum(x => (int)x.SoLuong), 1, short.MaxValue)
            })
            .ToList();
    }

    private static List<CongThucMonReadModel> QueryCongThucMonRows(CaPheDbContext context, IReadOnlyCollection<int> dsMonId)
    {
        if (dsMonId.Count == 0)
        {
            return new List<CongThucMonReadModel>();
        }

        return context.CongThucMon
            .Where(x => dsMonId.Contains(x.MonID))
            .Select(x => new CongThucMonReadModel
            {
                MonID = x.MonID,
                NguyenLieuID = x.NguyenLieuID,
                SoLuong = x.SoLuong
            })
            .ToList();
    }

    private static BanActionResultDTO? KiemTraDayDuCongThuc(
        IReadOnlyCollection<int> dsMonId,
        IEnumerable<CongThucMonReadModel> dsCongThuc)
    {
        var dsMonCoCongThuc = dsCongThuc
            .Select(x => x.MonID)
            .Distinct()
            .ToHashSet();

        foreach (var monId in dsMonId)
        {
            if (!dsMonCoCongThuc.Contains(monId))
            {
                return BusMessageCatalog.CreateActionResult(false, "Món chưa có công thức chế biến, không thể gọi món.");
            }
        }

        return null;
    }

    private static List<NhuCauNguyenLieuReadModel> TinhNhuCauNguyenLieu(
        IEnumerable<BanHangThemMonDTO> dsMonThem,
        IEnumerable<CongThucMonReadModel> dsCongThuc)
    {
        var soLuongMonTheoId = dsMonThem
            .ToDictionary(x => x.MonID, x => (decimal)x.SoLuong);

        return dsCongThuc
            .GroupBy(x => x.NguyenLieuID)
            .Select(g => new NhuCauNguyenLieuReadModel
            {
                NguyenLieuID = g.Key,
                SoLuongCan = g.Sum(x => x.SoLuong * soLuongMonTheoId[x.MonID])
            })
            .Where(x => x.SoLuongCan > 0)
            .ToList();
    }

    private static List<NhuCauNguyenLieuReadModel> TinhNhuCauHoanTonChoMon(
        int monId,
        short soLuongMon,
        IEnumerable<CongThucMonReadModel> dsCongThuc)
    {
        var heSoSoLuong = (decimal)soLuongMon;

        return dsCongThuc
            .Where(x => x.MonID == monId)
            .GroupBy(x => x.NguyenLieuID)
            .Select(g => new NhuCauNguyenLieuReadModel
            {
                NguyenLieuID = g.Key,
                SoLuongCan = g.Sum(x => x.SoLuong * heSoSoLuong)
            })
            .Where(x => x.SoLuongCan > 0)
            .ToList();
    }

    private static List<NhuCauNguyenLieuReadModel> TinhBienDongTonKhoDoiMon(
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauHoanTon,
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauMonMoi)
    {
        var soLuongHoanTheoId = dsNhuCauHoanTon
            .GroupBy(x => x.NguyenLieuID)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.SoLuongCan));

        var soLuongTruTheoId = dsNhuCauMonMoi
            .GroupBy(x => x.NguyenLieuID)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.SoLuongCan));

        var dsNguyenLieuId = soLuongHoanTheoId.Keys
            .Union(soLuongTruTheoId.Keys)
            .Distinct();

        return dsNguyenLieuId
            .Select(nguyenLieuId =>
            {
                soLuongHoanTheoId.TryGetValue(nguyenLieuId, out var soLuongHoan);
                soLuongTruTheoId.TryGetValue(nguyenLieuId, out var soLuongTru);

                return new NhuCauNguyenLieuReadModel
                {
                    NguyenLieuID = nguyenLieuId,
                    SoLuongCan = soLuongTru - soLuongHoan
                };
            })
            .Where(x => x.SoLuongCan != 0)
            .ToList();
    }

    private static BanActionResultDTO? KiemTraTonKho(
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauNguyenLieu,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb)
    {
        foreach (var nhuCau in dsNhuCauNguyenLieu)
        {
            if (!dsNguyenLieuDb.TryGetValue(nhuCau.NguyenLieuID, out var nguyenLieu))
            {
                return BusMessageCatalog.CreateActionResult(false, "Công thức món tham chiếu nguyên liệu không tồn tại.");
            }

            if (nguyenLieu.SoLuongTon < nhuCau.SoLuongCan)
            {
                return BusMessageCatalog.CreateActionResult(false, "Nguyên liệu không đủ tồn kho để gọi món.");
            }
        }

        return null;
    }

    private static BanActionResultDTO? KiemTraTonKhoTheoBienDong(
        IEnumerable<NhuCauNguyenLieuReadModel> dsBienDongTonKho,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb)
    {
        foreach (var bienDong in dsBienDongTonKho)
        {
            if (bienDong.SoLuongCan <= 0)
            {
                continue;
            }

            if (!dsNguyenLieuDb.TryGetValue(bienDong.NguyenLieuID, out var nguyenLieu))
            {
                return BusMessageCatalog.CreateActionResult(false, "Công thức món tham chiếu nguyên liệu không tồn tại.");
            }

            if (nguyenLieu.SoLuongTon < bienDong.SoLuongCan)
            {
                return BusMessageCatalog.CreateActionResult(false, "Nguyên liệu không đủ tồn kho để đổi món.");
            }
        }

        return null;
    }

    private static void TruTonKhoNguyenLieu(
        CaPheDbContext context,
        int hoaDonId,
        int banId,
        int nhanVienId,
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauNguyenLieu,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb)
    {
        var thoiDiemXuat = DateTime.Now;
        var lyDoXuat = TaoLyDoXuatKhoTuThemMon(hoaDonId, banId);
        var dsChiTietPhieuXuat = new List<dtaChiTietPhieuXuat>();

        foreach (var nhuCau in dsNhuCauNguyenLieu)
        {
            if (!dsNguyenLieuDb.TryGetValue(nhuCau.NguyenLieuID, out var nguyenLieu))
            {
                continue;
            }

            var soLuongXuat = decimal.Round(nhuCau.SoLuongCan, 3, MidpointRounding.AwayFromZero);
            if (soLuongXuat <= 0)
            {
                continue;
            }

            nguyenLieu.SoLuongTon = Math.Max(0, nguyenLieu.SoLuongTon - soLuongXuat);
            nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
            nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);

            dsChiTietPhieuXuat.Add(new dtaChiTietPhieuXuat
            {
                NguyenLieuID = nhuCau.NguyenLieuID,
                SoLuong = soLuongXuat
            });
        }

        if (dsChiTietPhieuXuat.Count == 0)
        {
            return;
        }

        var phieuXuat = new dtaPhieuXuatKho
        {
            NgayXuat = thoiDiemXuat,
            LyDo = lyDoXuat,
            NhanVienID = nhanVienId
        };

        foreach (var chiTiet in dsChiTietPhieuXuat)
        {
            chiTiet.PhieuXuat = phieuXuat;
        }

        context.PhieuXuatKho.Add(phieuXuat);
        context.ChiTietPhieuXuat.AddRange(dsChiTietPhieuXuat);
    }

    private static void HoanTonKhoNguyenLieu(
        CaPheDbContext context,
        int nhanVienId,
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauNguyenLieu,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb,
        string ghiChuNhapKho)
    {
        var thoiDiemNhap = DateTime.Now;
        var dsChiTietPhieuNhap = new List<dtaChiTietPhieuNhap>();

        foreach (var nhuCau in dsNhuCauNguyenLieu)
        {
            if (!dsNguyenLieuDb.TryGetValue(nhuCau.NguyenLieuID, out var nguyenLieu))
            {
                continue;
            }

            var soLuongNhap = decimal.Round(Math.Abs(nhuCau.SoLuongCan), 3, MidpointRounding.AwayFromZero);
            if (soLuongNhap <= 0)
            {
                continue;
            }

            var donGiaNhap = decimal.Round(nguyenLieu.GiaNhapGanNhat, 2, MidpointRounding.AwayFromZero);

            nguyenLieu.SoLuongTon += soLuongNhap;
            nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
            nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);

            dsChiTietPhieuNhap.Add(new dtaChiTietPhieuNhap
            {
                NguyenLieuID = nhuCau.NguyenLieuID,
                SoLuong = soLuongNhap,
                DonGiaNhap = donGiaNhap,
                ThanhTien = decimal.Round(soLuongNhap * donGiaNhap, 2, MidpointRounding.AwayFromZero)
            });
        }

        if (dsChiTietPhieuNhap.Count == 0)
        {
            return;
        }

        var phieuNhap = new dtaPhieuNhapKho
        {
            NgayNhap = thoiDiemNhap,
            GhiChu = ghiChuNhapKho,
            NhanVienID = nhanVienId
        };

        foreach (var chiTiet in dsChiTietPhieuNhap)
        {
            chiTiet.PhieuNhap = phieuNhap;
        }

        context.PhieuNhapKho.Add(phieuNhap);
        context.ChiTietPhieuNhap.AddRange(dsChiTietPhieuNhap);
    }

    private static void ApDungBienDongTonKhoDoiMon(
        CaPheDbContext context,
        int hoaDonId,
        int nhanVienId,
        int monCuId,
        int monMoiId,
        IEnumerable<NhuCauNguyenLieuReadModel> dsBienDongTonKho,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb)
    {
        var thoiDiem = DateTime.Now;
        var lyDoXuat = TaoLyDoXuatKhoTuDoiMon(hoaDonId, monCuId, monMoiId);
        var ghiChuNhap = TaoGhiChuNhapKhoTuDoiMon(hoaDonId, monCuId, monMoiId);
        var dsChiTietPhieuXuat = new List<dtaChiTietPhieuXuat>();
        var dsChiTietPhieuNhap = new List<dtaChiTietPhieuNhap>();

        foreach (var bienDong in dsBienDongTonKho)
        {
            if (!dsNguyenLieuDb.TryGetValue(bienDong.NguyenLieuID, out var nguyenLieu))
            {
                continue;
            }

            if (bienDong.SoLuongCan > 0)
            {
                var soLuongXuat = decimal.Round(bienDong.SoLuongCan, 3, MidpointRounding.AwayFromZero);
                if (soLuongXuat <= 0)
                {
                    continue;
                }

                nguyenLieu.SoLuongTon = Math.Max(0, nguyenLieu.SoLuongTon - soLuongXuat);
                dsChiTietPhieuXuat.Add(new dtaChiTietPhieuXuat
                {
                    NguyenLieuID = bienDong.NguyenLieuID,
                    SoLuong = soLuongXuat
                });
            }
            else
            {
                var soLuongNhap = decimal.Round(Math.Abs(bienDong.SoLuongCan), 3, MidpointRounding.AwayFromZero);
                if (soLuongNhap <= 0)
                {
                    continue;
                }

                var donGiaNhap = decimal.Round(nguyenLieu.GiaNhapGanNhat, 2, MidpointRounding.AwayFromZero);

                nguyenLieu.SoLuongTon += soLuongNhap;
                dsChiTietPhieuNhap.Add(new dtaChiTietPhieuNhap
                {
                    NguyenLieuID = bienDong.NguyenLieuID,
                    SoLuong = soLuongNhap,
                    DonGiaNhap = donGiaNhap,
                    ThanhTien = decimal.Round(soLuongNhap * donGiaNhap, 2, MidpointRounding.AwayFromZero)
                });
            }

            nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
            nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);
        }

        if (dsChiTietPhieuXuat.Count > 0)
        {
            var phieuXuat = new dtaPhieuXuatKho
            {
                NgayXuat = thoiDiem,
                LyDo = lyDoXuat,
                NhanVienID = nhanVienId
            };

            foreach (var chiTiet in dsChiTietPhieuXuat)
            {
                chiTiet.PhieuXuat = phieuXuat;
            }

            context.PhieuXuatKho.Add(phieuXuat);
            context.ChiTietPhieuXuat.AddRange(dsChiTietPhieuXuat);
        }

        if (dsChiTietPhieuNhap.Count > 0)
        {
            var phieuNhap = new dtaPhieuNhapKho
            {
                NgayNhap = thoiDiem,
                GhiChu = ghiChuNhap,
                NhanVienID = nhanVienId
            };

            foreach (var chiTiet in dsChiTietPhieuNhap)
            {
                chiTiet.PhieuNhap = phieuNhap;
            }

            context.PhieuNhapKho.Add(phieuNhap);
            context.ChiTietPhieuNhap.AddRange(dsChiTietPhieuNhap);
        }
    }

    private static void CapNhatTrangThaiBan(CaPheDbContext context, int banId, int trangThaiMoi)
    {
        if (banId <= 0)
        {
            return;
        }

        var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
        if (ban == null)
        {
            return;
        }

        ban.TrangThai = trangThaiMoi;
    }

    private static bool LaBanMangDi(CaPheDbContext context, int banId)
    {
        if (banId <= 0)
        {
            return false;
        }

        return context.Ban
            .AsNoTracking()
            .Any(x => x.ID == banId && x.TenBan == TakeAwayTableName);
    }

    private static string TaoLyDoXuatKhoTuThemMon(int hoaDonId, int banId)
    {
        return $"Xuất kho phục vụ gọi món cho hóa đơn HD{hoaDonId:D5} (bàn {banId:D2}).";
    }

    private static string TaoGhiChuNhapKhoTuXoaMon(int hoaDonId, int monId)
    {
        return $"Hoàn tồn kho do xóa món {monId} khỏi hóa đơn HD{hoaDonId:D5}.";
    }

    private static string TaoGhiChuNhapKhoTuHuyHoaDon(int hoaDonId)
    {
        return $"Hoàn tồn kho do hủy hóa đơn HD{hoaDonId:D5}.";
    }

    private static string TaoLyDoXuatKhoTuDoiMon(int hoaDonId, int monCuId, int monMoiId)
    {
        return $"Xuất kho do đổi món {monCuId} -> {monMoiId} cho hóa đơn HD{hoaDonId:D5}.";
    }

    private static string TaoGhiChuNhapKhoTuDoiMon(int hoaDonId, int monCuId, int monMoiId)
    {
        return $"Hoàn tồn kho do đổi món {monCuId} -> {monMoiId} cho hóa đơn HD{hoaDonId:D5}.";
    }

    private static int TinhTrangThaiNguyenLieu(decimal soLuongTon, decimal mucCanhBao, int trangThaiHienTai)
    {
        if (trangThaiHienTai == 0)
        {
            return 0;
        }

        if (soLuongTon <= 0)
        {
            return 2;
        }

        if (soLuongTon <= mucCanhBao)
        {
            return 2;
        }

        return 1;
    }

    private static string ChuyenTrangThaiNguyenLieuTextLegacy(int trangThai, decimal soLuongTon)
    {
        return trangThai switch
        {
            0 => "Ngừng dùng",
            2 => soLuongTon <= 0 ? "Hết hàng" : "Sắp hết",
            _ => soLuongTon <= 0 ? "Hết hàng" : "Đang sử dụng"
        };
    }

    private sealed class CongThucMonReadModel
    {
        public int MonID { get; init; }
        public int NguyenLieuID { get; init; }
        public decimal SoLuong { get; init; }
    }

    private sealed class NhuCauNguyenLieuReadModel
    {
        public int NguyenLieuID { get; init; }
        public decimal SoLuongCan { get; init; }
    }

    private sealed class HoaDonSnapshot
    {
        public int HoaDonId { get; init; }
        public int TrangThai { get; init; }
        public decimal TongTien { get; init; }
        public int TongMon { get; init; }
        public List<HoaDonSnapshotItem> ChiTiet { get; init; } = new();
    }

    private sealed class HoaDonSnapshotItem
    {
        public int MonID { get; init; }
        public short SoLuong { get; init; }
        public decimal DonGia { get; init; }
        public decimal ThanhTien { get; init; }
    }

    private sealed class PendingActivityLog
    {
        public int? UserId { get; init; }
        public string Action { get; init; } = string.Empty;
        public string EntityName { get; init; } = string.Empty;
        public string EntityId { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public object? OldValue { get; init; }
        public object? NewValue { get; init; }
        public string? PerformedBy { get; init; }
    }
}
