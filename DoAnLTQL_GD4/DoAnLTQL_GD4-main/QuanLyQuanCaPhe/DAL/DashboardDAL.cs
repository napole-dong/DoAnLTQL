using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class DashboardDAL
{
    public DashboardTongQuanDTO GetTongQuan(DateTime today, DateTime tomorrow)
    {
        using var context = new CaPheDbContext();

        var doanhThuHomNay = context.HoaDon_ChiTiet
            .AsNoTracking()
            .Where(ct => ct.HoaDon.TrangThai == 1 && ct.HoaDon.NgayLap >= today && ct.HoaDon.NgayLap < tomorrow)
            .Sum(ct => (long)ct.SoLuongBan * ct.DonGiaBan);

        var soHoaDonHomNay = context.HoaDon
            .AsNoTracking()
            .Count(hd => hd.NgayLap >= today && hd.NgayLap < tomorrow);

        var tongBan = context.Ban.AsNoTracking().Count();
        var banDangSuDung = context.Ban.AsNoTracking().Count(b => b.TrangThai == 1);

        var soMonBanHomNay = context.HoaDon_ChiTiet
            .AsNoTracking()
            .Where(ct => ct.HoaDon.NgayLap >= today && ct.HoaDon.NgayLap < tomorrow)
            .Sum(ct => (int?)ct.SoLuongBan) ?? 0;

        return new DashboardTongQuanDTO
        {
            DoanhThuHomNay = doanhThuHomNay,
            SoHoaDonHomNay = soHoaDonHomNay,
            TongBan = tongBan,
            BanDangSuDung = banDangSuDung,
            SoMonBanHomNay = soMonBanHomNay
        };
    }

    public List<DashboardRevenueDTO> GetDoanhThuTheoNgay(DateTime fromDate, DateTime toDate)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon_ChiTiet
            .AsNoTracking()
            .Where(ct => ct.HoaDon.TrangThai == 1 && ct.HoaDon.NgayLap >= fromDate && ct.HoaDon.NgayLap < toDate)
            .GroupBy(ct => ct.HoaDon.NgayLap.Date)
            .Select(g => new DashboardRevenueDTO
            {
                Ngay = g.Key,
                DoanhThu = g.Sum(ct => (long)ct.SoLuongBan * ct.DonGiaBan)
            })
            .ToList();
    }

    public List<DashboardTopMonDTO> GetTopMonBanChay(DateTime fromDate, DateTime toDate, int top = 5)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon_ChiTiet
            .AsNoTracking()
            .Where(ct => ct.HoaDon.NgayLap >= fromDate && ct.HoaDon.NgayLap < toDate)
            .GroupBy(ct => ct.Mon.TenMon)
            .Select(g => new DashboardTopMonDTO
            {
                TenMon = g.Key,
                SoLuong = g.Sum(ct => (int)ct.SoLuongBan)
            })
            .OrderByDescending(x => x.SoLuong)
            .Take(top)
            .ToList();
    }

    public List<DashboardRecentBillDTO> GetHoaDonGanDay(int soLuong)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon
            .AsNoTracking()
            .OrderByDescending(hd => hd.NgayLap)
            .Select(hd => new DashboardRecentBillDTO
            {
                ID = hd.ID,
                NgayLap = hd.NgayLap,
                TrangThai = hd.TrangThai,
                TenBan = hd.Ban.TenBan,
                TenNhanVien = hd.NhanVien.HoVaTen,
                TongTien = hd.HoaDon_ChiTiet.Sum(ct => (long)ct.SoLuongBan * ct.DonGiaBan)
            })
            .Take(Math.Clamp(soLuong, 5, 10))
            .ToList();
    }
}
