using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class ThongKeDAL : IThongKeRepository
{
    public List<ThongKeHoaDonDTO> LayDanhSachHoaDonDaThanhToan(DateTime tuNgay, DateTime denNgay, string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        return BuildPaidInvoiceQuery(context, tuNgay, denNgay, tuKhoa)
            .OrderByDescending(x => x.NgayLap)
            .ThenByDescending(x => x.ID)
            .Select(x => new ThongKeHoaDonDTO
            {
                HoaDonId = x.ID,
                NgayLap = x.NgayLap,
                KhachHangID = x.KhachHangID,
                TongTien = x.TongTien
            })
            .ToList();
    }

    public List<ThongKeTopMonDTO> LayTopMonBanChay(DateTime tuNgay, DateTime denNgay, string? tuKhoa, int soLuongTop)
    {
        using var context = new CaPheDbContext();

        var hoaDonDaThanhToanQuery = BuildPaidInvoiceQuery(context, tuNgay, denNgay, tuKhoa)
            .Select(x => x.ID);

        return (
            from ct in context.HoaDon_ChiTiet
                .AsNoTracking()
                .IgnoreQueryFilters()
            join hd in hoaDonDaThanhToanQuery on ct.HoaDonID equals hd
            join mon in context.Mon
                .AsNoTracking()
                .IgnoreQueryFilters() on ct.MonID equals mon.ID
            group new { ct, mon } by new { ct.MonID, mon.TenMon }
            into grouped
            orderby grouped.Sum(x => (int)x.ct.SoLuongBan) descending,
                grouped.Sum(x => x.ct.ThanhTien) descending,
                grouped.Key.TenMon
            select new ThongKeTopMonDTO
            {
                MonID = grouped.Key.MonID,
                TenMon = grouped.Key.TenMon,
                SoLuongBan = grouped.Sum(x => (int)x.ct.SoLuongBan),
                DoanhThu = grouped.Sum(x => x.ct.ThanhTien)
            })
            .Take(soLuongTop)
            .ToList();
    }

    public int LaySoHoaDonHuy(DateTime tuNgay, DateTime denNgay, string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        return BuildCancelledInvoiceQuery(context, tuNgay, denNgay, tuKhoa).Count();
    }

    private static IQueryable<dtaHoadon> BuildPaidInvoiceQuery(
        CaPheDbContext context,
        DateTime tuNgay,
        DateTime denNgay,
        string? tuKhoa)
    {
        return BuildInvoiceQueryByTrangThai(
            context,
            tuNgay,
            denNgay,
            tuKhoa,
            (int)HoaDonTrangThai.Paid,
            (int)HoaDonTrangThai.Closed);
    }

    private static IQueryable<dtaHoadon> BuildCancelledInvoiceQuery(
        CaPheDbContext context,
        DateTime tuNgay,
        DateTime denNgay,
        string? tuKhoa)
    {
        return BuildInvoiceQueryByTrangThai(
            context,
            tuNgay,
            denNgay,
            tuKhoa,
            (int)HoaDonTrangThai.Cancelled);
    }

    private static IQueryable<dtaHoadon> BuildInvoiceQueryByTrangThai(
        CaPheDbContext context,
        DateTime tuNgay,
        DateTime denNgay,
        string? tuKhoa,
        params int[] trangThaiHopLe)
    {
        var tuNgayDate = tuNgay.Date;
        var denNgayDate = denNgay.Date.AddDays(1).AddTicks(-1);

        if (trangThaiHopLe == null || trangThaiHopLe.Length == 0)
        {
            return context.HoaDon
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(_ => false);
        }

        var query = context.HoaDon
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => trangThaiHopLe.Contains(x.TrangThai)
                && x.NgayLap >= tuNgayDate
                && x.NgayLap <= denNgayDate);

        if (string.IsNullOrWhiteSpace(tuKhoa))
        {
            return query;
        }

        var keyword = tuKhoa.Trim();
        var keywordPattern = $"%{keyword}%";
        var hasKeywordId = int.TryParse(keyword, out var keywordId);

        return query.Where(x =>
            (hasKeywordId && x.ID == keywordId)
            || EF.Functions.Like(x.Ban.TenBan, keywordPattern)
            || EF.Functions.Like(x.CustomerName, keywordPattern)
            || EF.Functions.Like(x.NhanVien.HoVaTen, keywordPattern)
            || x.HoaDon_ChiTiet.Any(ct => EF.Functions.Like(ct.Mon.TenMon, keywordPattern)));
    }
}
