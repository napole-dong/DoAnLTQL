using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class HoaDonDAL
{
    public List<HoaDonDTO> GetDanhSachHoaDon(HoaDonFilterDTO boLoc)
    {
        using var context = new CaPheDbContext();

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
            query = query.Where(x =>
                x.ID.ToString().Contains(tuKhoa)
                || x.Ban.TenBan.Contains(tuKhoa)
                || (x.KhachHang != null && x.KhachHang.HoVaTen.Contains(tuKhoa))
                || x.NhanVien.HoVaTen.Contains(tuKhoa));
        }

        return query
            .OrderByDescending(x => x.NgayLap)
            .ThenByDescending(x => x.ID)
            .Select(x => new HoaDonDTO
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

    public HoaDonDTO? GetHoaDonTheoId(int hoaDonId)
    {
        using var context = new CaPheDbContext();

        var hoaDon = context.HoaDon
            .AsNoTracking()
            .Include(x => x.Ban)
            .Include(x => x.KhachHang)
            .Include(x => x.NhanVien)
            .Include(x => x.HoaDon_ChiTiet)
            .ThenInclude(x => x.Mon)
            .FirstOrDefault(x => x.ID == hoaDonId);

        if (hoaDon == null)
        {
            return null;
        }

        var chiTiet = hoaDon.HoaDon_ChiTiet
            .OrderBy(x => x.Mon.TenMon)
            .Select(x => new HoaDonChiTietDTO
            {
                MonID = x.MonID,
                TenMon = x.Mon.TenMon,
                SoLuong = x.SoLuongBan,
                DonGia = x.DonGiaBan
            })
            .ToList();

        return new HoaDonDTO
        {
            ID = hoaDon.ID,
            NgayLap = hoaDon.NgayLap,
            BanID = hoaDon.BanID,
            TenBan = hoaDon.Ban.TenBan,
            KhachHangID = hoaDon.KhachHangID ?? 0,
            TenKhachHang = hoaDon.KhachHang?.HoVaTen ?? "Khách lẻ",
            NhanVienID = hoaDon.NhanVienID,
            TenNhanVien = hoaDon.NhanVien.HoVaTen,
            TrangThai = hoaDon.TrangThai,
            TongTien = chiTiet.Sum(x => x.ThanhTien),
            ChiTiet = chiTiet
        };
    }

    public int GetNextHoaDonId()
    {
        using var context = new CaPheDbContext();
        return (context.HoaDon.Max(x => (int?)x.ID) ?? 0) + 1;
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
        using var context = new CaPheDbContext();

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

        return (true, "Tạo hóa đơn mới thành công.", hoaDon.ID);
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
        using var context = new CaPheDbContext();

        var hoaDon = context.HoaDon
            .Include(x => x.HoaDon_ChiTiet)
            .FirstOrDefault(x => x.ID == hoaDonId);

        if (hoaDon == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy hóa đơn cần thu tiền." };
        }

        if (hoaDon.TrangThai != 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Hóa đơn không ở trạng thái chờ thanh toán." };
        }

        if (hoaDon.HoaDon_ChiTiet.Count == 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Hóa đơn chưa có món, không thể thu tiền." };
        }

        var banId = hoaDon.BanID;
        hoaDon.TrangThai = 1;
        context.SaveChanges();

        DongBoTrangThaiBanTheoHoaDonMo(context, banId);
        context.SaveChanges();

        return new BanActionResultDTO
        {
            ThanhCong = true,
            ThongBao = $"Đã xác nhận thu tiền cho hóa đơn HD{hoaDon.ID:D5}."
        };
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
        var nhanVien = context.NhanVien.OrderBy(x => x.ID).FirstOrDefault();
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
        var khach = context.KhachHang.FirstOrDefault(x => x.HoVaTen == "Khách lẻ")
                    ?? context.KhachHang.OrderBy(x => x.ID).FirstOrDefault();

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
}
