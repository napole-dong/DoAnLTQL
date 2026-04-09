using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class HoaDonDAL
{
    public List<HoaDonDTO> GetDanhSachHoaDon(HoaDonFilterDTO boLoc)
    {
        using var context = new CaPheDbContext();

        var hoaDonRows = QueryDanhSachHoaDonRows(context, boLoc);
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
                TrangThaiBan = x.TrangThai
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
        using var context = new CaPheDbContext();
        using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

        AppLogger.Info($"Start ThemHoaDon. BanID={request.BanID}.", nameof(HoaDonDAL));

        try
        {
            var ban = context.Ban.FirstOrDefault(x => x.ID == request.BanID);
            if (ban == null)
            {
                return (false, "Không tìm thấy bàn đã chọn.", 0);
            }

            var coHoaDonMo = context.HoaDon.Any(x => x.BanID == request.BanID && x.TrangThai == 0);
            if (coHoaDonMo)
            {
                return (false, "Bàn đang có hóa đơn chưa thanh toán.", 0);
            }

            var nhanVienId = GetOrCreateNhanVienMacDinh(context);
            var khachHangId = GetOrCreateKhachLe(context);

            var hoaDon = new dtaHoadon
            {
                BanID = request.BanID,
                NhanVienID = nhanVienId,
                KhachHangID = khachHangId,
                NgayLap = request.NgayLap,
                TrangThai = 0,
                GhiChuHoaDon = string.Empty
            };

            context.HoaDon.Add(hoaDon);
            ban.TrangThai = 1;
            context.SaveChanges();
            transaction.Commit();

            return (true, "Tạo hóa đơn mới thành công.", hoaDon.ID);
        }
        catch (DbUpdateException ex) when (LaLoiTrungHoaDonMo(ex))
        {
            transaction.Rollback();
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
            transaction.Rollback();

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

        if (hoaDon.TrangThai != 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Chỉ được sửa hóa đơn chưa thanh toán." };
        }

        var banMoi = context.Ban.FirstOrDefault(x => x.ID == request.BanID);
        if (banMoi == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy bàn đã chọn." };
        }

        var coHoaDonMoKhac = context.HoaDon.Any(x =>
            x.BanID == request.BanID
            && x.TrangThai == 0
            && x.ID != request.ID);

        if (coHoaDonMoKhac)
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
        context.SaveChanges();

        return new BanActionResultDTO { ThanhCong = true, ThongBao = "Cập nhật hóa đơn thành công." };
    }

    public BanActionResultDTO ThemMonVaoHoaDon(int hoaDonId, int monId, short soLuong)
    {
        using var context = new CaPheDbContext();

        var hoaDon = context.HoaDon
            .Include(x => x.HoaDon_ChiTiet)
            .FirstOrDefault(x => x.ID == hoaDonId);

        if (hoaDon == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy hóa đơn để thêm món." };
        }

        if (hoaDon.TrangThai != 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Chỉ thêm món cho hóa đơn chưa thanh toán." };
        }

        var mon = context.Mon.FirstOrDefault(x => x.ID == monId);
        if (mon == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy món đã chọn." };
        }

        if (mon.TrangThai != 1)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Món đang ngừng bán, vui lòng chọn món khác." };
        }

        var chiTiet = hoaDon.HoaDon_ChiTiet
            .FirstOrDefault(x => x.MonID == monId && x.DonGiaBan == mon.DonGia && x.GhiChu == null);

        if (chiTiet == null)
        {
            context.HoaDon_ChiTiet.Add(new dtHoaDon_ChiTiet
            {
                HoaDonID = hoaDon.ID,
                MonID = monId,
                SoLuongBan = soLuong,
                DonGiaBan = mon.DonGia,
                GhiChu = null
            });
        }
        else
        {
            chiTiet.SoLuongBan = (short)Math.Clamp(chiTiet.SoLuongBan + soLuong, 1, short.MaxValue);
        }

        var ban = context.Ban.FirstOrDefault(x => x.ID == hoaDon.BanID);
        if (ban != null)
        {
            ban.TrangThai = 1;
        }

        context.SaveChanges();

        return new BanActionResultDTO { ThanhCong = true, ThongBao = "Thêm món vào hóa đơn thành công." };
    }

    public BanActionResultDTO HuyHoaDon(int hoaDonId)
    {
        using var context = new CaPheDbContext();

        var hoaDon = context.HoaDon.FirstOrDefault(x => x.ID == hoaDonId);
        if (hoaDon == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy hóa đơn cần hủy." };
        }

        if (hoaDon.TrangThai == 1)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Hóa đơn đã thanh toán, không thể hủy." };
        }

        if (hoaDon.TrangThai == 2)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Hóa đơn đã ở trạng thái hủy." };
        }

        var banId = hoaDon.BanID;
        hoaDon.TrangThai = 2;
        hoaDon.GhiChuHoaDon = $"[Hủy {DateTime.Now:dd/MM/yyyy HH:mm}] {hoaDon.GhiChuHoaDon}".Trim();
        context.SaveChanges();

        DongBoTrangThaiBanTheoHoaDonMo(context, banId);
        context.SaveChanges();

        return new BanActionResultDTO { ThanhCong = true, ThongBao = "Hủy hóa đơn thành công." };
    }

    public BanActionResultDTO XacNhanThuTien(int hoaDonId)
    {
        using var correlationScope = CorrelationContext.BeginScope();
        using var context = new CaPheDbContext();

        AppLogger.Audit(
            "Payment.Confirm.Start",
            "Bat dau xac nhan thu tien theo hoa don.",
            new { HoaDonId = hoaDonId },
            nameof(HoaDonDAL));

        BanActionResultDTO TuChoi(string thongBao)
        {
            AppLogger.Audit(
                "Payment.Confirm.Rejected",
                thongBao,
                new { HoaDonId = hoaDonId },
                nameof(HoaDonDAL));

            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }

        try
        {
            var hoaDon = context.HoaDon
                .Include(x => x.HoaDon_ChiTiet)
                .FirstOrDefault(x => x.ID == hoaDonId);

            if (hoaDon == null)
            {
                return TuChoi("Không tìm thấy hóa đơn cần thu tiền.");
            }

            if (hoaDon.TrangThai != 0)
            {
                return TuChoi("Hóa đơn không ở trạng thái chờ thanh toán.");
            }

            if (!hoaDon.HoaDon_ChiTiet.Any())
            {
                return TuChoi("Hóa đơn chưa có món, không thể thu tiền.");
            }

            var tongTien = hoaDon.HoaDon_ChiTiet.Sum(x => x.SoLuongBan * x.DonGiaBan);

            var banId = hoaDon.BanID;
            hoaDon.TrangThai = 1;
            context.SaveChanges();

            DongBoTrangThaiBanTheoHoaDonMo(context, banId);
            context.SaveChanges();

            AppLogger.Audit(
                "Payment.Confirm.Success",
                "Xac nhan thu tien thanh cong.",
                new
                {
                    HoaDonId = hoaDon.ID,
                    BanId = banId,
                    TongTien = tongTien
                },
                nameof(HoaDonDAL));

            return new BanActionResultDTO
            {
                ThanhCong = true,
                ThongBao = $"Đã xác nhận thu tiền cho hóa đơn HD{hoaDon.ID:D5}."
            };
        }
        catch (Exception ex)
        {
            var mappedError = AppExceptionMapper.Map(ex);
            AppLogger.Error(
                ex,
                $"Unexpected failure in XacNhanThuTien. HoaDonID={hoaDonId}.",
                nameof(HoaDonDAL),
                mappedError.Code);
            AppLogger.Audit(
                "Payment.Confirm.Failed",
                "Xac nhan thu tien that bai do loi he thong.",
                new { HoaDonId = hoaDonId },
                nameof(HoaDonDAL));

            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Không thể xác nhận thu tiền do lỗi hệ thống.",
                ex);
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }
    }

    private static List<HoaDonListReadModel> QueryDanhSachHoaDonRows(CaPheDbContext context, HoaDonFilterDTO boLoc)
    {
        var query = BuildHoaDonFilterQuery(context, boLoc);

        return query
            .OrderByDescending(x => x.NgayLap)
            .ThenByDescending(x => x.ID)
            .Select(x => new HoaDonListReadModel
            {
                ID = x.ID,
                NgayLap = x.NgayLap,
                BanID = x.BanID,
                TenBan = x.Ban.TenBan,
                KhachHangID = x.KhachHangID ?? 0,
                TenKhachHang = x.KhachHang != null ? x.KhachHang.HoVaTen : "Khách lẻ",
                NhanVienID = x.NhanVienID,
                TenNhanVien = x.NhanVien.HoVaTen,
                TrangThai = x.TrangThai,
                TongTien = x.HoaDon_ChiTiet.Sum(ct => ct.SoLuongBan * ct.DonGiaBan)
            })
            .ToList();
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
            query = query.Where(x => x.TrangThai == boLoc.TrangThai.Value);
        }

        if (!string.IsNullOrWhiteSpace(boLoc.TuKhoa))
        {
            var tuKhoa = boLoc.TuKhoa.Trim();
            var keywordPattern = $"%{tuKhoa}%";
            var hasKeywordId = int.TryParse(tuKhoa, out var keywordId);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.Ban.TenBan, keywordPattern)
                || (x.KhachHang != null && EF.Functions.Like(x.KhachHang.HoVaTen, keywordPattern))
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
                TongTien = x.TongTien
            })
            .ToList();
    }

    private static HoaDonHeaderReadModel? QueryHoaDonHeaderRow(CaPheDbContext context, int hoaDonId)
    {
        return context.HoaDon
            .AsNoTracking()
            .Where(x => x.ID == hoaDonId)
            .Select(x => new HoaDonHeaderReadModel
            {
                ID = x.ID,
                NgayLap = x.NgayLap,
                BanID = x.BanID,
                TenBan = x.Ban.TenBan,
                KhachHangID = x.KhachHangID ?? 0,
                TenKhachHang = x.KhachHang != null ? x.KhachHang.HoVaTen : "Khách lẻ",
                NhanVienID = x.NhanVienID,
                TenNhanVien = x.NhanVien.HoVaTen,
                TrangThai = x.TrangThai
            })
            .FirstOrDefault();
    }

    private static List<HoaDonChiTietReadModel> QueryHoaDonChiTietRows(CaPheDbContext context, int hoaDonId)
    {
        return context.HoaDon_ChiTiet
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

        ban.TrangThai = context.HoaDon.Any(x => x.BanID == banId && x.TrangThai == 0) ? 1 : 0;
    }

    private static int GetOrCreateNhanVienMacDinh(CaPheDbContext context)
    {
        var nhanVien = context.NhanVien
            .AsNoTracking()
            .OrderBy(x => x.ID)
            .FirstOrDefault();
        if (nhanVien != null)
        {
            return nhanVien.ID;
        }

        nhanVien = new dtaNhanVien
        {
            HoVaTen = "Nhân viên bán hàng",
            DienThoai = null,
            DiaChi = null
        };

        context.NhanVien.Add(nhanVien);
        context.SaveChanges();

        return nhanVien.ID;
    }

    private static int GetOrCreateKhachLe(CaPheDbContext context)
    {
        var khach = context.KhachHang
                        .AsNoTracking()
                        .FirstOrDefault(x => x.HoVaTen == "Khách lẻ");

        if (khach != null)
        {
            return khach.ID;
        }

        khach = new dtaKhachHang
        {
            HoVaTen = "Khách lẻ",
            DienThoai = null,
            DiaChi = null
        };

        context.KhachHang.Add(khach);
        context.SaveChanges();

        return khach.ID;
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
