using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class HoaDonDAL : IHoaDonRepository
{
    private const string InvoiceConcurrencyMessage = "Dữ liệu đã bị thay đổi bởi nhân viên khác. Vui lòng tải lại!";
    private const string WalkInCustomerName = "Khách lẻ";
    private readonly IOrderService _orderService;
    private readonly IActivityLogWriter _activityLogWriter;

    public HoaDonDAL(IOrderService? orderService = null, IActivityLogWriter? activityLogWriter = null)
    {
        _orderService = orderService ?? new OrderService();
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public List<HoaDonDTO> GetDanhSachHoaDon(HoaDonFilterDTO boLoc)
    {
        return GetDanhSachHoaDonAsync(boLoc)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    public async Task<List<HoaDonDTO>> GetDanhSachHoaDonAsync(HoaDonFilterDTO boLoc, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(boLoc);

        if (boLoc.PageNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(boLoc.PageNumber), "PageNumber phải lớn hơn 0.");
        }

        if (boLoc.PageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(boLoc.PageSize), "PageSize phải lớn hơn 0.");
        }

        await using var context = new CaPheDbContext();

        var hoaDonRows = await QueryDanhSachHoaDonRowsAsync(context, boLoc, cancellationToken).ConfigureAwait(false);
        return MapDanhSachHoaDonDtos(hoaDonRows);
    }

    public HoaDonDTO? GetHoaDonTheoId(int hoaDonId)
    {
        using var context = new CaPheDbContext();

        var hoaDonHeader = QueryHoaDonHeaderRow(context, hoaDonId);
        if (hoaDonHeader == null)
        {
            return null;
        }

        var hoaDonChiTietRows = QueryHoaDonChiTietRows(context, hoaDonId);
        return MapHoaDonTheoIdDto(hoaDonHeader, hoaDonChiTietRows);
    }

    public int GetNextHoaDonId()
    {
        using var context = new CaPheDbContext();
        return (context.HoaDon
            .AsNoTracking()
            .Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public List<HoaDonBanKhachItemDTO> GetDanhSachBanKhach()
    {
        using var context = new CaPheDbContext();

        return context.Ban
            .AsNoTracking()
            .OrderBy(x => x.ID)
            .Select(x => new HoaDonBanKhachItemDTO
            {
                BanID = x.ID,
                TenBan = x.TenBan,
                TrangThaiBan = x.TrangThai,
                TrangThaiHoaDonDangHoatDong = context.HoaDon
                    .Where(i => i.BanID == x.ID
                        && i.TrangThai != (int)HoaDonTrangThai.Closed
                        && i.TrangThai != (int)HoaDonTrangThai.Cancelled)
                    .OrderByDescending(i => i.NgayLap)
                    .ThenByDescending(i => i.ID)
                    .Select(i => (int?)i.TrangThai)
                    .FirstOrDefault()
            })
            .ToList();
    }

    public List<HoaDonMonItemDTO> GetDanhSachMonDangKinhDoanh()
    {
        using var context = new CaPheDbContext();

        return context.Mon
            .AsNoTracking()
            .Where(x => x.TrangThai == 1)
            .OrderBy(x => x.TenMon)
            .Select(x => new HoaDonMonItemDTO
            {
                MonID = x.ID,
                TenMon = x.TenMon,
                DonGia = x.DonGia
            })
            .ToList();
    }

    public (bool ThanhCong, string ThongBao, int HoaDonId) ThemHoaDon(HoaDonSaveRequestDTO request)
    {
        using var correlationScope = CorrelationContext.BeginScope();

        AppLogger.Info($"Start ThemHoaDon. BanID={request.BanID}.", nameof(HoaDonDAL));

        try
        {
            var ketQua = ExecutionStrategyTransactionRunner.ExecuteAsync(
                async context =>
            {
                var ban = await context.Ban
                    .FirstOrDefaultAsync(x => x.ID == request.BanID)
                    .ConfigureAwait(false);
                if (ban == null)
                {
                    return (false, "Không tìm thấy bàn đã chọn.", 0);
                }

                if (ban.TrangThai != 0)
                {
                    return (false, "Bàn chưa được dọn. Vui lòng dọn bàn trước khi tạo hóa đơn mới.", 0);
                }

                var coHoaDonDangHoatDong = await context.HoaDon
                    .AnyAsync(x => x.BanID == request.BanID
                        && x.TrangThai != (int)HoaDonTrangThai.Closed
                        && x.TrangThai != (int)HoaDonTrangThai.Cancelled)
                    .ConfigureAwait(false);
                if (coHoaDonDangHoatDong)
                {
                    return (false, "Bàn chưa được dọn. Vui lòng dọn bàn trước khi tạo hóa đơn mới.", 0);
                }

                if (!TryLayNhanVienDangNhapHopLe(context, out var nhanVienId, out var thongBaoNhanVien))
                {
                    return (false, thongBaoNhanVien, 0);
                }

                var khachHangId = request.KhachHangID;
                var customerName = WalkInCustomerName;

                if (khachHangId.HasValue)
                {
                    if (khachHangId.Value <= 0)
                    {
                        khachHangId = null;
                    }
                    else
                    {
                        var khachHang = await context.KhachHang
                            .AsNoTracking()
                            .Where(x => x.ID == khachHangId.Value)
                            .Select(x => new
                            {
                                x.ID,
                                x.HoVaTen
                            })
                            .FirstOrDefaultAsync()
                            .ConfigureAwait(false);

                        if (khachHang == null)
                        {
                            return (false, "Khách hàng đã chọn không tồn tại hoặc đã bị xóa.", 0);
                        }

                        khachHangId = khachHang.ID;
                        customerName = string.IsNullOrWhiteSpace(khachHang.HoVaTen)
                            ? WalkInCustomerName
                            : khachHang.HoVaTen;
                    }
                }

                var hoaDon = new dtaHoadon
                {
                    BanID = request.BanID,
                    NhanVienID = nhanVienId,
                    KhachHangID = khachHangId,
                    CustomerName = customerName,
                    NgayLap = request.NgayLap,
                    TrangThai = (int)HoaDonTrangThai.Open,
                    TongTien = 0m,
                    GhiChuHoaDon = string.Empty
                };

                context.HoaDon.Add(hoaDon);
                ban.TrangThai = 1;
                await context.SaveChangesAsync().ConfigureAwait(false);

                return (true, "Tạo hóa đơn mới thành công.", hoaDon.ID);
            }, result => result.Item1, System.Data.IsolationLevel.Serializable)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            if (ketQua.Item1 && ketQua.Item3 > 0)
            {
                var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
                _activityLogWriter.Log(
                    nguoiDung?.UserId,
                    AuditActions.CreateInvoice,
                    "Invoice",
                    ketQua.Item3.ToString(),
                    $"Đã tạo hóa đơn HD{ketQua.Item3:D5} cho bàn {request.BanID}.",
                    oldValue: null,
                    newValue: new
                    {
                        HoaDonId = ketQua.Item3,
                        request.BanID,
                        request.KhachHangID,
                        request.NgayLap,
                        TrangThai = HoaDonTrangThai.Open.ToString()
                    },
                    performedBy: nguoiDung?.TenDangNhap);
            }

            return (ketQua.Item1, ketQua.Item2, ketQua.Item3);
        }
        catch (DbUpdateException ex) when (LaLoiTrungHoaDonMo(ex))
        {
            AppLogger.Warning(
                $"Open invoice conflict while ThemHoaDon. BanID={request.BanID}. {ex.Message}",
                nameof(HoaDonDAL),
                AppErrorCode.DbDuplicateKey);

            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Bàn đang có hóa đơn chưa thanh toán.",
                ex);

            return (false, thongBao, 0);
        }
        catch (Exception ex)
        {
            var mappedError = AppExceptionMapper.Map(ex);
            AppLogger.Error(
                ex,
                $"Unexpected failure in ThemHoaDon. BanID={request.BanID}.",
                nameof(HoaDonDAL),
                mappedError.Code);
            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Không thể tạo hóa đơn do lỗi hệ thống.",
                ex);
            return (false, thongBao, 0);
        }
    }

    public BanActionResultDTO CapNhatHoaDon(HoaDonSaveRequestDTO request)
    {
        using var context = new CaPheDbContext();

        var hoaDon = context.HoaDon.FirstOrDefault(x => x.ID == request.ID);
        if (hoaDon == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy hóa đơn để cập nhật." };
        }

        if (request.RowVersion == null || request.RowVersion.Length == 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = InvoiceConcurrencyMessage };
        }

        context.Entry(hoaDon)
            .Property(x => x.RowVersion)
            .OriginalValue = request.RowVersion;

        if (!HoaDonStateMachine.IsOpen(hoaDon.TrangThai))
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Chỉ được sửa hóa đơn Open." };
        }

        var oldSnapshot = new
        {
            hoaDon.ID,
            hoaDon.BanID,
            hoaDon.KhachHangID,
            hoaDon.NgayLap,
            hoaDon.TrangThai,
            hoaDon.TongTien,
            hoaDon.GhiChuHoaDon
        };

        var banMoi = context.Ban.FirstOrDefault(x => x.ID == request.BanID);
        if (banMoi == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy bàn đã chọn." };
        }

        if (banMoi.TrangThai != 0 && banMoi.ID != hoaDon.BanID)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Bàn đích chưa được dọn, chưa thể chuyển hóa đơn sang bàn này." };
        }

        var coHoaDonDangHoatDongKhac = context.HoaDon.Any(x =>
            x.BanID == request.BanID
            && x.TrangThai != (int)HoaDonTrangThai.Closed
            && x.TrangThai != (int)HoaDonTrangThai.Cancelled
            && x.ID != request.ID);

        if (coHoaDonDangHoatDongKhac)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Bàn đã có hóa đơn mở khác." };
        }

        var banCuId = hoaDon.BanID;
        hoaDon.BanID = request.BanID;
        hoaDon.NgayLap = request.NgayLap;

        if (banCuId != request.BanID)
        {
            DongBoTrangThaiBanTheoHoaDonMo(context, banCuId);
        }

        banMoi.TrangThai = 1;

        try
        {
            context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            using var reloadContext = new CaPheDbContext();
            _ = reloadContext.HoaDon
                .AsNoTracking()
                .FirstOrDefault(x => x.ID == request.ID);

            return new BanActionResultDTO { ThanhCong = false, ThongBao = InvoiceConcurrencyMessage };
        }

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.UpdateInvoice,
            "Invoice",
            hoaDon.ID.ToString(),
            TaoMoTaCapNhatHoaDon(hoaDon.ID, banCuId, request.BanID, oldSnapshot.NgayLap, hoaDon.NgayLap),
            oldValue: oldSnapshot,
            newValue: new
            {
                hoaDon.ID,
                hoaDon.BanID,
                hoaDon.KhachHangID,
                hoaDon.NgayLap,
                hoaDon.TrangThai,
                hoaDon.TongTien,
                hoaDon.GhiChuHoaDon
            },
            performedBy: nguoiDung?.TenDangNhap);

        return new BanActionResultDTO { ThanhCong = true, ThongBao = "Cập nhật hóa đơn thành công." };
    }

    public BanActionResultDTO ThemMonVaoHoaDon(int hoaDonId, int monId, short soLuong, byte[]? rowVersion = null)
    {
        return _orderService.AddItemToOrder(hoaDonId, monId, soLuong, rowVersion);
    }

    public BanActionResultDTO CapNhatSoLuongMonTrongHoaDon(int hoaDonId, int monId, short soLuong, byte[]? rowVersion = null)
    {
        return _orderService.UpdateItemQuantity(hoaDonId, monId, soLuong, rowVersion);
    }

    public BanActionResultDTO HuyHoaDon(int hoaDonId, byte[]? rowVersion = null)
    {
        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap()?.TenDangNhap ?? "system";
        return _orderService.VoidInvoice(hoaDonId, "Hủy hóa đơn", nguoiDung, rowVersion);
    }

    public BanActionResultDTO HuyHoaDon(int hoaDonId, string reason, string user, byte[]? rowVersion = null)
    {
        return _orderService.VoidInvoice(hoaDonId, reason, user, rowVersion);
    }

    public BanActionResultDTO XacNhanThuTien(int hoaDonId, byte[]? rowVersion = null)
    {
        return _orderService.Checkout(hoaDonId, rowVersion);
    }

    public BanActionResultDTO CapNhatKhachHangChoHoaDonMo(int hoaDonId, int? khachHangId, byte[]? rowVersion = null)
    {
        if (hoaDonId <= 0)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Vui lòng chọn hóa đơn hợp lệ."
            };
        }

        if (rowVersion == null || rowVersion.Length == 0)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Thiếu RowVersion hợp lệ."
            };
        }

        using var context = new CaPheDbContext();

        var hoaDon = context.HoaDon.FirstOrDefault(x => x.ID == hoaDonId);
        if (hoaDon == null)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Không tìm thấy hóa đơn cần cập nhật khách hàng."
            };
        }

        context.Entry(hoaDon)
            .Property(x => x.RowVersion)
            .OriginalValue = rowVersion;

        if (!HoaDonStateMachine.IsOpen(hoaDon.TrangThai))
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Chỉ cập nhật khách hàng cho hóa đơn Open."
            };
        }

        var oldSnapshot = new
        {
            hoaDon.ID,
            hoaDon.KhachHangID,
            hoaDon.CustomerName,
            hoaDon.NgayLap,
            hoaDon.TrangThai,
            hoaDon.TongTien
        };

        var khachHangIdHopLe = khachHangId.HasValue && khachHangId.Value > 0
            ? khachHangId.Value
            : (int?)null;

        var tenKhachHang = WalkInCustomerName;

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
                return new BanActionResultDTO
                {
                    ThanhCong = false,
                    ThongBao = "Khách hàng đã chọn không tồn tại hoặc đã bị xóa."
                };
            }

            khachHangIdHopLe = khachHang.ID;
            tenKhachHang = string.IsNullOrWhiteSpace(khachHang.HoVaTen)
                ? WalkInCustomerName
                : khachHang.HoVaTen;
        }

        hoaDon.KhachHangID = khachHangIdHopLe;
        hoaDon.CustomerName = tenKhachHang;

        try
        {
            context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = InvoiceConcurrencyMessage
            };
        }

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.UpdateInvoice,
            "Invoice",
            hoaDon.ID.ToString(),
            $"Đã cập nhật khách hàng cho hóa đơn HD{hoaDon.ID:D5}: {oldSnapshot.CustomerName} -> {hoaDon.CustomerName}.",
            oldValue: oldSnapshot,
            newValue: new
            {
                hoaDon.ID,
                hoaDon.KhachHangID,
                hoaDon.CustomerName,
                hoaDon.NgayLap,
                hoaDon.TrangThai,
                hoaDon.TongTien
            },
            performedBy: nguoiDung?.TenDangNhap);

        return new BanActionResultDTO
        {
            ThanhCong = true,
            ThongBao = "Cập nhật khách hàng cho hóa đơn thành công."
        };
    }

    private static string TaoMoTaCapNhatHoaDon(int hoaDonId, int banCuId, int banMoiId, DateTime ngayLapCu, DateTime ngayLapMoi)
    {
        var moTa = $"Đã cập nhật hóa đơn HD{hoaDonId:D5}.";
        if (banCuId != banMoiId)
        {
            moTa += $" Chuyển bàn: {banCuId} -> {banMoiId}.";
        }

        if (ngayLapCu != ngayLapMoi)
        {
            moTa += $" Ngày lập: {ngayLapCu:dd/MM/yyyy HH:mm} -> {ngayLapMoi:dd/MM/yyyy HH:mm}.";
        }

        return moTa;
    }

    private static List<HoaDonListReadModel> QueryDanhSachHoaDonRows(CaPheDbContext context, HoaDonFilterDTO boLoc)
    {
        return BuildDanhSachHoaDonProjection(context, boLoc)
            .ToList();
    }

    private static Task<List<HoaDonListReadModel>> QueryDanhSachHoaDonRowsAsync(
        CaPheDbContext context,
        HoaDonFilterDTO boLoc,
        CancellationToken cancellationToken)
    {
        return BuildDanhSachHoaDonProjection(context, boLoc)
            .ToListAsync(cancellationToken);
    }

    private static IQueryable<HoaDonListReadModel> BuildDanhSachHoaDonProjection(CaPheDbContext context, HoaDonFilterDTO boLoc)
    {
        var query = BuildHoaDonFilterQuery(context, boLoc);
        var skip = (boLoc.PageNumber - 1) * boLoc.PageSize;

        return query
            .OrderByDescending(x => x.NgayLap)
            .ThenByDescending(x => x.ID)
            .Skip(skip)
            .Take(boLoc.PageSize)
            .Select(x => new HoaDonListReadModel
            {
                ID = x.ID,
                NgayLap = x.NgayLap,
                BanID = x.BanID,
                TenBan = x.Ban.TenBan,
                KhachHangID = x.KhachHangID ?? 0,
                TenKhachHang = x.CustomerName ?? WalkInCustomerName,
                NhanVienID = x.NhanVienID,
                TenNhanVien = x.NhanVien.HoVaTen,
                TrangThai = x.TrangThai,
                RowVersion = x.RowVersion,
                TongTien = x.HoaDon_ChiTiet
                    .Where(ct => ct.SoLuongBan > 0)
                    .Select(ct => (decimal?)ct.ThanhTien)
                    .Sum() ?? 0m
            });
    }

    private static IQueryable<dtaHoadon> BuildHoaDonFilterQuery(CaPheDbContext context, HoaDonFilterDTO boLoc)
    {
        var tuNgay = boLoc.TuNgay.Date;
        var denNgay = boLoc.DenNgay.Date.AddDays(1).AddTicks(-1);

        var query = context.HoaDon
            .AsNoTracking()
            .Where(x => x.NgayLap >= tuNgay && x.NgayLap <= denNgay);

        if (boLoc.TrangThai.HasValue)
        {
            if (boLoc.TrangThai.Value == (int)HoaDonTrangThai.Voided)
            {
                query = query.Where(x => x.TrangThai == (int)HoaDonTrangThai.Voided || x.TrangThai == (int)HoaDonTrangThai.Closed);
            }
            else
            {
                query = query.Where(x => x.TrangThai == boLoc.TrangThai.Value);
            }
        }

        if (!string.IsNullOrWhiteSpace(boLoc.TuKhoa))
        {
            var tuKhoa = boLoc.TuKhoa.Trim();
            var keywordPattern = $"%{tuKhoa}%";
            var hasKeywordId = int.TryParse(tuKhoa, out var keywordId);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.Ban.TenBan, keywordPattern)
                || EF.Functions.Like(x.CustomerName, keywordPattern)
                || EF.Functions.Like(x.NhanVien.HoVaTen, keywordPattern));
        }

        return query;
    }

    private static List<HoaDonDTO> MapDanhSachHoaDonDtos(IEnumerable<HoaDonListReadModel> hoaDonRows)
    {
        return hoaDonRows
            .Select(x => new HoaDonDTO
            {
                ID = x.ID,
                NgayLap = x.NgayLap,
                BanID = x.BanID,
                TenBan = x.TenBan,
                KhachHangID = x.KhachHangID,
                TenKhachHang = x.TenKhachHang,
                NhanVienID = x.NhanVienID,
                TenNhanVien = x.TenNhanVien,
                TrangThai = x.TrangThai,
                RowVersion = x.RowVersion,
                TongTien = x.TongTien
            })
            .ToList();
    }

    private static HoaDonHeaderReadModel? QueryHoaDonHeaderRow(CaPheDbContext context, int hoaDonId)
    {
        return context.HoaDon
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.ID == hoaDonId)
            .Select(x => new HoaDonHeaderReadModel
            {
                ID = x.ID,
                NgayLap = x.NgayLap,
                BanID = x.BanID,
                TenBan = x.Ban.TenBan,
                KhachHangID = x.KhachHangID ?? 0,
                TenKhachHang = x.CustomerName ?? WalkInCustomerName,
                NhanVienID = x.NhanVienID,
                TenNhanVien = x.NhanVien.HoVaTen,
                TrangThai = x.TrangThai,
                TongTien = x.HoaDon_ChiTiet
                    .Where(ct => ct.SoLuongBan > 0)
                    .Select(ct => (decimal?)ct.ThanhTien)
                    .Sum() ?? 0m,
                RowVersion = x.RowVersion
            })
            .FirstOrDefault();
    }

    private static List<HoaDonChiTietReadModel> QueryHoaDonChiTietRows(CaPheDbContext context, int hoaDonId)
    {
        return context.HoaDon_ChiTiet
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.HoaDonID == hoaDonId)
            .OrderBy(x => x.Mon.TenMon)
            .Select(x => new HoaDonChiTietReadModel
            {
                MonID = x.MonID,
                TenMon = x.Mon.TenMon,
                SoLuong = x.SoLuongBan,
                DonGia = x.DonGiaBan
            })
            .ToList();
    }

    private static HoaDonDTO MapHoaDonTheoIdDto(HoaDonHeaderReadModel hoaDonHeader, IEnumerable<HoaDonChiTietReadModel> chiTietRows)
    {
        var chiTietDtos = chiTietRows
            .Select(x => new HoaDonChiTietDTO
            {
                MonID = x.MonID,
                TenMon = x.TenMon,
                SoLuong = x.SoLuong,
                DonGia = x.DonGia
            })
            .ToList();

        return new HoaDonDTO
        {
            ID = hoaDonHeader.ID,
            NgayLap = hoaDonHeader.NgayLap,
            BanID = hoaDonHeader.BanID,
            TenBan = hoaDonHeader.TenBan,
            KhachHangID = hoaDonHeader.KhachHangID,
            TenKhachHang = hoaDonHeader.TenKhachHang,
            NhanVienID = hoaDonHeader.NhanVienID,
            TenNhanVien = hoaDonHeader.TenNhanVien,
            TrangThai = hoaDonHeader.TrangThai,
            RowVersion = hoaDonHeader.RowVersion,
            TongTien = chiTietDtos.Sum(x => x.ThanhTien),
            ChiTiet = chiTietDtos
        };
    }

    private sealed class HoaDonListReadModel
    {
        public int ID { get; init; }
        public DateTime NgayLap { get; init; }
        public int BanID { get; init; }
        public string TenBan { get; init; } = string.Empty;
        public int KhachHangID { get; init; }
        public string TenKhachHang { get; init; } = string.Empty;
        public int NhanVienID { get; init; }
        public string TenNhanVien { get; init; } = string.Empty;
        public int TrangThai { get; init; }
        public byte[] RowVersion { get; init; } = Array.Empty<byte>();
        public decimal TongTien { get; init; }
    }

    private sealed class HoaDonHeaderReadModel
    {
        public int ID { get; init; }
        public DateTime NgayLap { get; init; }
        public int BanID { get; init; }
        public string TenBan { get; init; } = string.Empty;
        public int KhachHangID { get; init; }
        public string TenKhachHang { get; init; } = string.Empty;
        public int NhanVienID { get; init; }
        public string TenNhanVien { get; init; } = string.Empty;
        public int TrangThai { get; init; }
        public decimal TongTien { get; init; }
        public byte[] RowVersion { get; init; } = Array.Empty<byte>();
    }

    private sealed class HoaDonChiTietReadModel
    {
        public int MonID { get; init; }
        public string TenMon { get; init; } = string.Empty;
        public short SoLuong { get; init; }
        public decimal DonGia { get; init; }
    }

    private static void DongBoTrangThaiBanTheoHoaDonMo(CaPheDbContext context, int banId)
    {
        var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
        if (ban == null)
        {
            return;
        }

        ban.TrangThai = context.HoaDon.Any(x => x.BanID == banId
            && x.TrangThai != (int)HoaDonTrangThai.Closed
            && x.TrangThai != (int)HoaDonTrangThai.Cancelled)
            ? 1
            : 0;
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

    private static bool LaLoiTrungHoaDonMo(DbUpdateException ex)
    {
        if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx)
        {
            return (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                && sqlEx.Message.Contains("UX_HoaDon_Ban_Mo", StringComparison.OrdinalIgnoreCase);
        }

        return ex.InnerException?.Message.Contains("UX_HoaDon_Ban_Mo", StringComparison.OrdinalIgnoreCase) == true;
    }
}
