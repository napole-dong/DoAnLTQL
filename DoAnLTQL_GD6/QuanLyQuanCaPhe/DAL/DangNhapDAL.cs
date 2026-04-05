using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.DAL;

public class DangNhapDAL
{
    public ThongTinDangNhapDTO? XacThucDangNhap(string tenDangNhap, string matKhau)
    {
        using var context = new CaPheDbContext();

        var user = context.User
            .Include(x => x.NhanVien)
            .Include(x => x.VaiTro)
            .FirstOrDefault(x => x.TenDangNhap == tenDangNhap && x.HoatDong);

        if (user == null)
        {
            return null;
        }

        if (!MatKhauService.KiemTraMatKhau(matKhau, user.MatKhau))
        {
            return null;
        }

        if (MatKhauService.CanNangCapHash(user.MatKhau))
        {
            user.MatKhau = MatKhauService.BamMatKhau(matKhau);
            context.SaveChanges();
        }

        return new ThongTinDangNhapDTO
        {
            UserId = user.ID,
            NhanVienId = user.NhanVienID,
            TenDangNhap = user.TenDangNhap,
            HoVaTen = user.NhanVien != null ? user.NhanVien.HoVaTen : user.TenDangNhap,
            QuyenHan = user.VaiTro != null ? user.VaiTro.TenVaiTro : "Nhân viên"
        };
    }

    public bool TonTaiTenDangNhap(string tenDangNhap)
    {
        using var context = new CaPheDbContext();
        return context.User.AsNoTracking().Any(x => x.TenDangNhap == tenDangNhap);
    }

    public bool LaTaiKhoanHoatDong(string tenDangNhap)
    {
        using var context = new CaPheDbContext();
        return context.User.AsNoTracking().Any(x => x.TenDangNhap == tenDangNhap && x.HoatDong);
    }
}
