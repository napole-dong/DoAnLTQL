using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.DAL;

public sealed class TaiKhoanMacDinhDAL : ITaiKhoanMacDinhRepository
{
    private static readonly IReadOnlyList<TaiKhoanMacDinhSpec> TaiKhoanMacDinh =
    [
        new TaiKhoanMacDinhSpec("admin", RoleEnum.Admin, "Tai khoan Admin"),
        new TaiKhoanMacDinhSpec("manager", RoleEnum.Manager, "Tai khoan Manager"),
        new TaiKhoanMacDinhSpec("staff", RoleEnum.Staff, "Tai khoan Staff")
    ];

    public sealed record KhoiTaoTaiKhoanMacDinhResult(int SoVaiTroTaoMoi, int SoTaiKhoanTaoMoi, int SoTaiKhoanCapNhat);

    public KhoiTaoTaiKhoanMacDinhResult DamBaoTaiKhoanMacDinh(string matKhauMacDinh)
    {
        if (string.IsNullOrWhiteSpace(matKhauMacDinh))
        {
            throw new ArgumentException("Mat khau mac dinh khong duoc de trong.", nameof(matKhauMacDinh));
        }

        using var context = new CaPheDbContext();

        var soVaiTroTaoMoi = 0;
        var soTaiKhoanTaoMoi = 0;
        var soTaiKhoanCapNhat = 0;

        var vaiTroMap = DamBaoVaiTroMacDinh(context, ref soVaiTroTaoMoi);

        foreach (var taiKhoan in TaiKhoanMacDinh)
        {
            var vaiTroId = vaiTroMap[taiKhoan.VaiTro];
            var user = context.User
                .FirstOrDefault(x => x.TenDangNhap == taiKhoan.TenDangNhap);

            if (user == null)
            {
                var nhanVien = TaoNhanVienMacDinh(taiKhoan.HoVaTen);
                context.NhanVien.Add(nhanVien);
                context.SaveChanges();

                context.User.Add(new dtaUser
                {
                    NhanVienID = nhanVien.ID,
                    VaiTroID = vaiTroId,
                    TenDangNhap = taiKhoan.TenDangNhap,
                    MatKhau = MatKhauService.BamMatKhau(matKhauMacDinh),
                    HoatDong = true
                });

                context.SaveChanges();
                soTaiKhoanTaoMoi++;
                continue;
            }

            var daCapNhat = false;

            if (user.VaiTroID != vaiTroId)
            {
                user.VaiTroID = vaiTroId;
                daCapNhat = true;
            }

            if (!user.HoatDong)
            {
                user.HoatDong = true;
                daCapNhat = true;
            }

            if (!MatKhauService.KiemTraMatKhau(matKhauMacDinh, user.MatKhau) || MatKhauService.CanNangCapHash(user.MatKhau))
            {
                user.MatKhau = MatKhauService.BamMatKhau(matKhauMacDinh);
                daCapNhat = true;
            }

            var nhanVienLienKet = context.NhanVien
                .IgnoreQueryFilters()
                .FirstOrDefault(x => x.ID == user.NhanVienID);

            if (nhanVienLienKet == null)
            {
                var nhanVienMoi = TaoNhanVienMacDinh(taiKhoan.HoVaTen);
                context.NhanVien.Add(nhanVienMoi);
                context.SaveChanges();

                user.NhanVienID = nhanVienMoi.ID;
                daCapNhat = true;
            }
            else
            {
                if (nhanVienLienKet.IsDeleted)
                {
                    nhanVienLienKet.IsDeleted = false;
                    nhanVienLienKet.DeletedAt = null;
                    nhanVienLienKet.DeletedBy = null;
                    daCapNhat = true;
                }

                if (string.IsNullOrWhiteSpace(nhanVienLienKet.HoVaTen))
                {
                    nhanVienLienKet.HoVaTen = taiKhoan.HoVaTen;
                    daCapNhat = true;
                }
            }

            if (!daCapNhat)
            {
                continue;
            }

            context.SaveChanges();
            soTaiKhoanCapNhat++;
        }

        return new KhoiTaoTaiKhoanMacDinhResult(soVaiTroTaoMoi, soTaiKhoanTaoMoi, soTaiKhoanCapNhat);
    }

    private static Dictionary<RoleEnum, int> DamBaoVaiTroMacDinh(CaPheDbContext context, ref int soVaiTroTaoMoi)
    {
        var ketQua = new Dictionary<RoleEnum, int>();

        foreach (var vaiTro in (RoleEnum[])Enum.GetValues(typeof(RoleEnum)))
        {
            var tenVaiTro = RoleMapper.ToRoleName(vaiTro);

            var duLieuVaiTro = context.VaiTro
                .FirstOrDefault(x => x.TenVaiTro == tenVaiTro);

            if (duLieuVaiTro == null)
            {
                duLieuVaiTro = new dtaVaiTro
                {
                    TenVaiTro = tenVaiTro,
                    MoTa = $"Default role {tenVaiTro}"
                };

                context.VaiTro.Add(duLieuVaiTro);
                context.SaveChanges();
                soVaiTroTaoMoi++;
            }

            ketQua[vaiTro] = duLieuVaiTro.ID;
        }

        return ketQua;
    }

    private static dtaNhanVien TaoNhanVienMacDinh(string hoVaTen)
    {
        return new dtaNhanVien
        {
            HoVaTen = hoVaTen,
            DienThoai = null,
            DiaChi = null
        };
    }

    private sealed record TaiKhoanMacDinhSpec(string TenDangNhap, RoleEnum VaiTro, string HoVaTen);
}