using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class CongThucDAL : ICongThucRepository
{
    public List<CongThucMonDTO> GetDanhSachCongThuc(string? tuKhoa, int? monId)
    {
        using var context = new CaPheDbContext();

        var query = context.CongThucMon
            .AsNoTracking()
            .AsQueryable();

        if (monId.HasValue && monId.Value > 0)
        {
            query = query.Where(x => x.MonID == monId.Value);
        }

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);

            query = query.Where(x =>
                (hasKeywordId && (x.MonID == keywordId || x.NguyenLieuID == keywordId))
                || EF.Functions.Like(x.Mon.TenMon, keywordPattern)
                || EF.Functions.Like(x.NguyenLieu.TenNguyenLieu, keywordPattern)
                || EF.Functions.Like(x.NguyenLieu.DonViTinh, keywordPattern));
        }

        return query
            .OrderBy(x => x.MonID)
            .ThenBy(x => x.NguyenLieuID)
            .Select(x => new CongThucMonDTO
            {
                MonID = x.MonID,
                TenMon = x.Mon.TenMon,
                NguyenLieuID = x.NguyenLieuID,
                TenNguyenLieu = x.NguyenLieu.TenNguyenLieu,
                SoLuong = x.SoLuong,
                DonViTinh = x.NguyenLieu.DonViTinh,
                SoLuongTon = x.NguyenLieu.SoLuongTon
            })
            .ToList();
    }

    public (bool ThanhCong, string ThongBao) ThemCongThuc(int monId, int nguyenLieuId, decimal soLuong)
    {
        using var context = new CaPheDbContext();

        if (!context.Mon.Any(x => x.ID == monId && !x.IsDeleted && x.TrangThai != 0))
        {
            return (false, "Món không tồn tại hoặc đã ngừng bán.");
        }

        if (!context.NguyenLieu.Any(x => x.ID == nguyenLieuId && x.TrangThai != 0))
        {
            return (false, "Nguyên liệu không tồn tại hoặc đã ngừng dùng.");
        }

        var daTonTai = context.CongThucMon.Any(x => x.MonID == monId && x.NguyenLieuID == nguyenLieuId);
        if (daTonTai)
        {
            return (false, "Công thức của món với nguyên liệu này đã tồn tại.");
        }

        context.CongThucMon.Add(new dtaCongThucMon
        {
            MonID = monId,
            NguyenLieuID = nguyenLieuId,
            SoLuong = soLuong
        });

        context.SaveChanges();
        return (true, "Thêm nguyên liệu vào công thức thành công.");
    }

    public (bool ThanhCong, string ThongBao) CapNhatCongThuc(int monId, int nguyenLieuId, decimal soLuong)
    {
        using var context = new CaPheDbContext();

        var congThuc = context.CongThucMon.FirstOrDefault(x => x.MonID == monId && x.NguyenLieuID == nguyenLieuId);
        if (congThuc == null)
        {
            return (false, "Không tìm thấy công thức để cập nhật.");
        }

        congThuc.SoLuong = soLuong;
        context.SaveChanges();
        return (true, "Cập nhật công thức thành công.");
    }

    public (bool ThanhCong, string ThongBao) XoaCongThuc(int monId, int nguyenLieuId)
    {
        using var context = new CaPheDbContext();

        var congThuc = context.CongThucMon.FirstOrDefault(x => x.MonID == monId && x.NguyenLieuID == nguyenLieuId);
        if (congThuc == null)
        {
            return (false, "Không tìm thấy công thức để xóa.");
        }

        context.CongThucMon.Remove(congThuc);
        context.SaveChanges();
        return (true, "Đã xóa nguyên liệu khỏi công thức.");
    }
}
