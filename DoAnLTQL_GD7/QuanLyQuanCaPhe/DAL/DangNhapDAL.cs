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

        var userRow = QueryNguoiDungDangNhap(context, tenDangNhap);
        if (userRow == null)
        {
            return null;
        }

        if (!MatKhauService.KiemTraMatKhau(matKhau, userRow.MatKhau))
        {
            return null;
        }

        if (MatKhauService.CanNangCapHash(userRow.MatKhau))
        {
            NangCapHashMatKhau(context, userRow.UserId, matKhau);
        }

        return MapThongTinDangNhapDto(userRow);
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

    private static DangNhapReadModel? QueryNguoiDungDangNhap(CaPheDbContext context, string tenDangNhap)
    {
        return context.User
            .AsNoTracking()
            .Where(x => x.TenDangNhap == tenDangNhap && x.HoatDong)
            .Select(x => new DangNhapReadModel
            {
                UserId = x.ID,
                NhanVienId = x.NhanVienID,
                RoleId = x.VaiTroID,
                TenDangNhap = x.TenDangNhap,
                MatKhau = x.MatKhau,
                HoVaTen = x.NhanVien != null ? x.NhanVien.HoVaTen : x.TenDangNhap,
                QuyenHan = x.VaiTro != null ? x.VaiTro.TenVaiTro : "Staff"
            })
            .FirstOrDefault();
    }

    private static void NangCapHashMatKhau(CaPheDbContext context, int userId, string matKhau)
    {
        var user = context.User.FirstOrDefault(x => x.ID == userId);
        if (user == null)
        {
            return;
        }

        user.MatKhau = MatKhauService.BamMatKhau(matKhau);
        context.SaveChanges();
    }

    private static ThongTinDangNhapDTO MapThongTinDangNhapDto(DangNhapReadModel userRow)
    {
        return new ThongTinDangNhapDTO
        {
            UserId = userRow.UserId,
            NhanVienId = userRow.NhanVienId,
            RoleId = userRow.RoleId,
            TenDangNhap = userRow.TenDangNhap,
            HoVaTen = userRow.HoVaTen,
            QuyenHan = userRow.QuyenHan
        };
    }

    private sealed class DangNhapReadModel
    {
        public int UserId { get; init; }
        public int NhanVienId { get; init; }
        public int RoleId { get; init; }
        public string TenDangNhap { get; init; } = string.Empty;
        public string MatKhau { get; init; } = string.Empty;
        public string HoVaTen { get; init; } = string.Empty;
        public string QuyenHan { get; init; } = string.Empty;
    }
}
