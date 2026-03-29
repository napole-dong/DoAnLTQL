using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class NguyenLieuDAL
{
    public List<NguyenLieuDTO> GetDanhSachNguyenLieu(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var query = context.Set<dtaNguyenLieu>()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            tuKhoa = tuKhoa.Trim();
            query = query.Where(x =>
                x.ID.ToString().Contains(tuKhoa)
                || x.TenNguyenLieu.Contains(tuKhoa)
                || x.DonViTinh.Contains(tuKhoa)
                || x.TrangThai.Contains(tuKhoa));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new NguyenLieuDTO
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
            TrangThai = nguyenLieuDTO.TrangThai
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
}
