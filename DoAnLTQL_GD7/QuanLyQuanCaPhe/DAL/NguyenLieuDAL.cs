using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class NguyenLieuDAL
{
    public List<NguyenLieuDTO> GetDanhSachNguyenLieu(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var nguyenLieuRows = QueryDanhSachNguyenLieuRows(context, tuKhoa);
        return MapNguyenLieuDtos(nguyenLieuRows);
    }

    public int GetNextNguyenLieuId()
    {
        using var context = new CaPheDbContext();
        return (context.Set<dtaNguyenLieu>().Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public NguyenLieuDTO ThemNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = new dtaNguyenLieu
        {
            TenNguyenLieu = nguyenLieuDTO.TenNguyenLieu,
            DonViTinh = nguyenLieuDTO.DonViTinh,
            SoLuongTon = nguyenLieuDTO.SoLuongTon,
            MucCanhBao = nguyenLieuDTO.MucCanhBao,
            GiaNhapGanNhat = nguyenLieuDTO.GiaNhapGanNhat,
            TrangThai = nguyenLieuDTO.TrangThai,
            TrangThaiTextLegacy = nguyenLieuDTO.TrangThaiHienThi
        };

        context.Set<dtaNguyenLieu>().Add(nguyenLieu);
        context.SaveChanges();

        nguyenLieuDTO.MaNguyenLieu = nguyenLieu.ID;
        return nguyenLieuDTO;
    }

    public bool CapNhatNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = context.Set<dtaNguyenLieu>().FirstOrDefault(x => x.ID == nguyenLieuDTO.MaNguyenLieu);
        if (nguyenLieu == null)
        {
            return false;
        }

        nguyenLieu.TenNguyenLieu = nguyenLieuDTO.TenNguyenLieu;
        nguyenLieu.DonViTinh = nguyenLieuDTO.DonViTinh;
        nguyenLieu.SoLuongTon = nguyenLieuDTO.SoLuongTon;
        nguyenLieu.MucCanhBao = nguyenLieuDTO.MucCanhBao;
        nguyenLieu.GiaNhapGanNhat = nguyenLieuDTO.GiaNhapGanNhat;
        nguyenLieu.TrangThai = nguyenLieuDTO.TrangThai;
        nguyenLieu.TrangThaiTextLegacy = nguyenLieuDTO.TrangThaiHienThi;

        context.SaveChanges();
        return true;
    }

    public bool XoaNguyenLieu(int maNguyenLieu)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = context.Set<dtaNguyenLieu>().FirstOrDefault(x => x.ID == maNguyenLieu);
        if (nguyenLieu == null)
        {
            return false;
        }

        context.Set<dtaNguyenLieu>().Remove(nguyenLieu);
        context.SaveChanges();
        return true;
    }

    private static List<NguyenLieuReadModel> QueryDanhSachNguyenLieuRows(CaPheDbContext context, string? tuKhoa)
    {
        var query = context.Set<dtaNguyenLieu>()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);
            var hasKeywordTrangThai = int.TryParse(keyword, out var keywordTrangThai);

            var matchNgungDung = "Ngừng dùng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchSapHet = "Sắp hết".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchHetHang = "Hết hàng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchDangSuDung = "Đang sử dụng".Contains(keyword, StringComparison.OrdinalIgnoreCase);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.TenNguyenLieu, keywordPattern)
                || EF.Functions.Like(x.DonViTinh, keywordPattern)
                || (hasKeywordTrangThai && x.TrangThai == keywordTrangThai)
                || (matchNgungDung && x.TrangThai == 0)
                || (matchSapHet && x.TrangThai == 2 && x.SoLuongTon > 0)
                || (matchHetHang && x.TrangThai != 0 && x.SoLuongTon <= 0)
                || (matchDangSuDung && x.TrangThai != 0 && x.TrangThai != 2 && x.SoLuongTon > 0));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new NguyenLieuReadModel
            {
                MaNguyenLieu = x.ID,
                TenNguyenLieu = x.TenNguyenLieu,
                DonViTinh = x.DonViTinh,
                SoLuongTon = x.SoLuongTon,
                MucCanhBao = x.MucCanhBao,
                GiaNhapGanNhat = x.GiaNhapGanNhat,
                TrangThai = x.TrangThai
            })
            .ToList();
    }

    private static List<NguyenLieuDTO> MapNguyenLieuDtos(IEnumerable<NguyenLieuReadModel> nguyenLieuRows)
    {
        return nguyenLieuRows
            .Select(MapNguyenLieuDto)
            .ToList();
    }

    private static NguyenLieuDTO MapNguyenLieuDto(NguyenLieuReadModel nguyenLieuRow)
    {
        return new NguyenLieuDTO
        {
            MaNguyenLieu = nguyenLieuRow.MaNguyenLieu,
            TenNguyenLieu = nguyenLieuRow.TenNguyenLieu,
            DonViTinh = nguyenLieuRow.DonViTinh,
            SoLuongTon = nguyenLieuRow.SoLuongTon,
            MucCanhBao = nguyenLieuRow.MucCanhBao,
            GiaNhapGanNhat = nguyenLieuRow.GiaNhapGanNhat,
            TrangThai = nguyenLieuRow.TrangThai
        };
    }

    private sealed class NguyenLieuReadModel
    {
        public int MaNguyenLieu { get; init; }
        public string TenNguyenLieu { get; init; } = string.Empty;
        public string DonViTinh { get; init; } = string.Empty;
        public decimal SoLuongTon { get; init; }
        public decimal MucCanhBao { get; init; }
        public decimal GiaNhapGanNhat { get; init; }
        public int TrangThai { get; init; }
    }
}
