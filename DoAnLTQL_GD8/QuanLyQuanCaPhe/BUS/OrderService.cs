using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class OrderService
{
    private const string GenericSystemErrorMessage = "Không thể xử lý hóa đơn do lỗi hệ thống.";
    private const string ConcurrencyErrorMessage = "Dữ liệu đã bị thay đổi bởi nhân viên khác. Vui lòng tải lại!";
    private const string InvoiceEntityName = "Invoice";
    private const string TakeAwayTableName = "Mang đi";

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

    internal BanActionResultDTO AddItemsToOrder(
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

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon) =>
        {
            if (hoaDon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
            {
                return BusMessageCatalog.CreateActionResult(false, "Chỉ thêm món cho hóa đơn chưa thanh toán.");
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
            TruTonKhoNguyenLieu(context, hoaDon.ID, hoaDon.BanID, dsNhuCauNguyenLieu, dsNguyenLieuDb);
            CapNhatTrangThaiBan(context, hoaDon.BanID, trangThaiMoi: 1);
            GhiAuditLog(context, "AddItem", hoaDon, snapshotCu, TaoSnapshotHoaDon(hoaDon));

            return BusMessageCatalog.CreateActionResult(true, successMessage);
        });
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

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon) =>
        {
            if (hoaDon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
            {
                return BusMessageCatalog.CreateActionResult(false, "Chỉ thêm món cho hóa đơn chưa thanh toán.");
            }

            var snapshotCu = TaoSnapshotHoaDon(hoaDon);

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
                dsNhuCauHoanTon,
                dsNguyenLieuDb,
                ghiChuNhapKho: TaoGhiChuNhapKhoTuXoaMon(hoaDon.ID, productId));

            CapNhatTrangThaiBan(context, hoaDon.BanID, trangThaiMoi: 1);
            GhiAuditLog(context, "RemoveItem", hoaDon, snapshotCu, TaoSnapshotHoaDon(hoaDon));

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

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon) =>
        {
            if (hoaDon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
            {
                return BusMessageCatalog.CreateActionResult(false, "Chỉ sửa món cho hóa đơn chưa thanh toán.");
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

            ApDungBienDongTonKhoDoiMon(context, hoaDon.ID, currentProductId, newProductId, dsBienDongTonKho, dsNguyenLieuDb);

            TinhLaiTongTienHoaDon(hoaDon);
            CapNhatTrangThaiBan(context, hoaDon.BanID, trangThaiMoi: 1);
            GhiAuditLog(context, "ReplaceItem", hoaDon, snapshotCu, TaoSnapshotHoaDon(hoaDon));

            return BusMessageCatalog.CreateActionResult(true, "Đổi món thành công.");
        });
    }

    public BanActionResultDTO CancelOrder(int orderId, byte[]? expectedRowVersion = null)
    {
        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap()?.TenDangNhap ?? "system";
        return CancelInvoice(orderId, "Hủy hóa đơn", nguoiDung, expectedRowVersion);
    }

    public BanActionResultDTO CancelInvoice(int orderId, string reason, string user, byte[]? expectedRowVersion = null)
    {
        if (orderId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn cần hủy.");
        }

        var lyDoHuy = string.IsNullOrWhiteSpace(reason) ? "Không có lý do" : reason.Trim();
        var nguoiHuy = string.IsNullOrWhiteSpace(user) ? "system" : user.Trim();

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon) =>
        {
            if (hoaDon.TrangThai != (int)HoaDonTrangThai.Draft)
            {
                return BusMessageCatalog.CreateActionResult(false, "Chỉ được hủy hóa đơn Draft.");
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
                var loiCongThuc = KiemTraDayDuCongThuc(dsMonId, dsCongThuc);
                if (loiCongThuc != null)
                {
                    return loiCongThuc;
                }

                var dsNhuCauHoanTon = TinhNhuCauNguyenLieu(dsMonTongHop, dsCongThuc);
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

                HoanTonKhoNguyenLieu(
                    context,
                    dsNhuCauHoanTon,
                    dsNguyenLieuDb,
                    ghiChuNhapKho: TaoGhiChuNhapKhoTuHuyHoaDon(hoaDon.ID));
            }

            hoaDon.TrangThai = (int)HoaDonTrangThai.Cancelled;
            hoaDon.GhiChuHoaDon = $"[Hủy {DateTime.Now:dd/MM/yyyy HH:mm}] {lyDoHuy}. {hoaDon.GhiChuHoaDon}".Trim();

            CapNhatTrangThaiBan(context, hoaDon.BanID, 0);
            TinhLaiTongTienHoaDon(hoaDon);
            GhiAuditLog(context, "Cancel", hoaDon, snapshotCu, TaoSnapshotHoaDon(hoaDon));

            context.AuditLog.Add(new dtaAuditLog
            {
                Action = "CANCEL_INVOICE",
                EntityName = InvoiceEntityName,
                EntityId = hoaDon.ID.ToString(CultureInfo.InvariantCulture),
                OldValue = JsonSerializer.Serialize(snapshotCu),
                NewValue = JsonSerializer.Serialize(new
                {
                    Reason = lyDoHuy,
                    CancelledBy = nguoiHuy,
                    Snapshot = TaoSnapshotHoaDon(hoaDon)
                }),
                PerformedBy = nguoiHuy,
                CreatedAt = DateTime.Now
            });

            return BusMessageCatalog.CreateActionResult(true, "Hủy hóa đơn thành công.");
        });
    }

    public BanActionResultDTO Checkout(int orderId, byte[]? expectedRowVersion = null)
    {
        if (orderId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi thu tiền.");
        }

        return ExecuteTransactional(orderId, expectedRowVersion, (context, hoaDon) =>
        {
            if (hoaDon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
            {
                return BusMessageCatalog.CreateActionResult(false, "Hóa đơn không ở trạng thái chờ thanh toán.");
            }

            var snapshotCu = TaoSnapshotHoaDon(hoaDon);
            TinhLaiTongTienHoaDon(hoaDon);

            if (hoaDon.TongTien <= 0)
            {
                return BusMessageCatalog.CreateActionResult(false, "Hóa đơn chưa có món, không thể thu tiền.");
            }

            hoaDon.TrangThai = (int)HoaDonTrangThai.Paid;
            GhiAuditLog(context, "Checkout", hoaDon, snapshotCu, TaoSnapshotHoaDon(hoaDon));

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
        Func<CaPheDbContext, dtaHoadon, BanActionResultDTO> processOrder)
    {
        try
        {
            using var strategyContext = new CaPheDbContext();
            var strategy = strategyContext.Database.CreateExecutionStrategy();

            var ketQua = BusMessageCatalog.CreateActionResult(false, GenericSystemErrorMessage);

            strategy.ExecuteAsync(async () =>
            {
                await using var context = new CaPheDbContext();
                await using var transaction = await context.Database
                    .BeginTransactionAsync(System.Data.IsolationLevel.Serializable)
                    .ConfigureAwait(false);

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

                ketQua = processOrder(context, hoaDon);
                if (!ketQua.ThanhCong)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    return;
                }

                try
                {
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    ketQua = BusMessageCatalog.CreateActionResult(false, ConcurrencyErrorMessage);
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    await TaiLaiHoaDonMoiNhat(orderId).ConfigureAwait(false);
                }
            }).ConfigureAwait(false).GetAwaiter().GetResult();

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

    private static void GhiAuditLog(
        CaPheDbContext context,
        string action,
        dtaHoadon hoaDon,
        HoaDonSnapshot snapshotCu,
        HoaDonSnapshot snapshotMoi)
    {
        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        var performedBy = string.IsNullOrWhiteSpace(nguoiDung?.TenDangNhap)
            ? "system"
            : nguoiDung!.TenDangNhap;

        context.AuditLog.Add(new dtaAuditLog
        {
            Action = action,
            EntityName = InvoiceEntityName,
            EntityId = hoaDon.ID.ToString(CultureInfo.InvariantCulture),
            OldValue = JsonSerializer.Serialize(snapshotCu),
            NewValue = JsonSerializer.Serialize(snapshotMoi),
            PerformedBy = performedBy,
            CreatedAt = DateTime.Now
        });
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
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauNguyenLieu,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb)
    {
        var thoiDiemXuat = DateTime.Now;
        var lyDoXuat = TaoLyDoXuatKhoTuThemMon(hoaDonId, banId);

        foreach (var nhuCau in dsNhuCauNguyenLieu)
        {
            if (!dsNguyenLieuDb.TryGetValue(nhuCau.NguyenLieuID, out var nguyenLieu))
            {
                continue;
            }

            nguyenLieu.SoLuongTon = Math.Max(0, nguyenLieu.SoLuongTon - nhuCau.SoLuongCan);
            nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
            nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);

            context.PhieuXuatKho.Add(new dtaPhieuXuatKho
            {
                NguyenLieuID = nhuCau.NguyenLieuID,
                SoLuongXuat = nhuCau.SoLuongCan,
                NgayXuat = thoiDiemXuat,
                LyDo = lyDoXuat
            });
        }
    }

    private static void HoanTonKhoNguyenLieu(
        CaPheDbContext context,
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauNguyenLieu,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb,
        string ghiChuNhapKho)
    {
        var thoiDiemNhap = DateTime.Now;

        foreach (var nhuCau in dsNhuCauNguyenLieu)
        {
            if (!dsNguyenLieuDb.TryGetValue(nhuCau.NguyenLieuID, out var nguyenLieu))
            {
                continue;
            }

            nguyenLieu.SoLuongTon += nhuCau.SoLuongCan;
            nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
            nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);

            context.PhieuNhapKho.Add(new dtaPhieuNhapKho
            {
                NguyenLieuID = nhuCau.NguyenLieuID,
                SoLuongNhap = nhuCau.SoLuongCan,
                GiaNhap = nguyenLieu.GiaNhapGanNhat,
                NgayNhap = thoiDiemNhap,
                GhiChu = ghiChuNhapKho
            });
        }
    }

    private static void ApDungBienDongTonKhoDoiMon(
        CaPheDbContext context,
        int hoaDonId,
        int monCuId,
        int monMoiId,
        IEnumerable<NhuCauNguyenLieuReadModel> dsBienDongTonKho,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb)
    {
        var thoiDiem = DateTime.Now;
        var lyDoXuat = TaoLyDoXuatKhoTuDoiMon(hoaDonId, monCuId, monMoiId);
        var ghiChuNhap = TaoGhiChuNhapKhoTuDoiMon(hoaDonId, monCuId, monMoiId);

        foreach (var bienDong in dsBienDongTonKho)
        {
            if (!dsNguyenLieuDb.TryGetValue(bienDong.NguyenLieuID, out var nguyenLieu))
            {
                continue;
            }

            if (bienDong.SoLuongCan > 0)
            {
                nguyenLieu.SoLuongTon = Math.Max(0, nguyenLieu.SoLuongTon - bienDong.SoLuongCan);
                context.PhieuXuatKho.Add(new dtaPhieuXuatKho
                {
                    NguyenLieuID = bienDong.NguyenLieuID,
                    SoLuongXuat = bienDong.SoLuongCan,
                    NgayXuat = thoiDiem,
                    LyDo = lyDoXuat
                });
            }
            else
            {
                var soLuongNhap = Math.Abs(bienDong.SoLuongCan);
                nguyenLieu.SoLuongTon += soLuongNhap;
                context.PhieuNhapKho.Add(new dtaPhieuNhapKho
                {
                    NguyenLieuID = bienDong.NguyenLieuID,
                    SoLuongNhap = soLuongNhap,
                    GiaNhap = nguyenLieu.GiaNhapGanNhat,
                    NgayNhap = thoiDiem,
                    GhiChu = ghiChuNhap
                });
            }

            nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
            nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);
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
}
