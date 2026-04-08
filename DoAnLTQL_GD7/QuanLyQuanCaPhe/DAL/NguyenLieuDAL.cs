using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class NguyenLieuDAL
{
    public List<NguyenLieuDTO> GetDanhSachNguyenLieu(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var dsNguyenLieu = context.Set<dtaNguyenLieu>()
            .AsNoTracking()
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

        if (string.IsNullOrWhiteSpace(tuKhoa))
        {
            return dsNguyenLieu;
        }

        tuKhoa = tuKhoa.Trim();
        return dsNguyenLieu
            .Where(x =>
                x.MaNguyenLieu.ToString().Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                || x.TenNguyenLieu.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                || x.DonViTinh.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                || x.TrangThaiHienThi.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                || x.TrangThai.ToString().Contains(tuKhoa, StringComparison.OrdinalIgnoreCase))
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
}
