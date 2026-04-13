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

    public KhoiTaoTaiKhoanMacDinhResult DamBaoTaiKhoanMacDinh(string? matKhauMacDinh)
    {
        using var context = new CaPheDbContext();

        var soVaiTroTaoMoi = 0;
        var soTaiKhoanTaoMoi = 0;
        var soTaiKhoanCapNhat = 0;

        var vaiTroMap = DamBaoVaiTroMacDinh(context, ref soVaiTroTaoMoi);
        var adminRoleId = vaiTroMap[RoleEnum.Admin];

        var daTonTaiTaiKhoanAdmin = context.User
            .AsNoTracking()
            .Any(x => x.VaiTroID == adminRoleId);

        var matKhauMacDinhChuan = string.IsNullOrWhiteSpace(matKhauMacDinh)
            ? null
            : matKhauMacDinh.Trim();
        var coMatKhauBootstrap = !string.IsNullOrWhiteSpace(matKhauMacDinhChuan);

        if (!daTonTaiTaiKhoanAdmin && !coMatKhauBootstrap)
        {
            throw new InvalidOperationException("Missing CAPHE_BOOTSTRAP_PASSWORD for first-time bootstrap.");
        }

        if (daTonTaiTaiKhoanAdmin && !coMatKhauBootstrap)
        {
            return new KhoiTaoTaiKhoanMacDinhResult(soVaiTroTaoMoi, 0, soTaiKhoanCapNhat);
        }

        var matKhauDaBam = MatKhauService.BamMatKhau(matKhauMacDinhChuan!);

        foreach (var taiKhoan in TaiKhoanMacDinh)
        {
            var userHienTai = context.User
                .FirstOrDefault(x => x.TenDangNhap == taiKhoan.TenDangNhap);

            if (userHienTai != null)
            {
                var daCapNhat = false;
                if (!MatKhauService.KiemTraMatKhau(matKhauMacDinhChuan!, userHienTai.MatKhau))
                {
                    userHienTai.MatKhau = matKhauDaBam;
                    daCapNhat = true;
                }

                var vaiTroMacDinh = vaiTroMap[taiKhoan.VaiTro];
                if (userHienTai.VaiTroID != vaiTroMacDinh)
                {
                    userHienTai.VaiTroID = vaiTroMacDinh;
                    daCapNhat = true;
                }

                if (!userHienTai.HoatDong)
                {
                    userHienTai.HoatDong = true;
                    daCapNhat = true;
                }

                if (daCapNhat)
                {
                    soTaiKhoanCapNhat++;
                }

                continue;
            }

            context.User.Add(new dtaUser
            {
                NhanVien = TaoNhanVienMacDinh(taiKhoan.HoVaTen),
                VaiTroID = vaiTroMap[taiKhoan.VaiTro],
                TenDangNhap = taiKhoan.TenDangNhap,
                MatKhau = matKhauDaBam,
                HoatDong = true
            });

            soTaiKhoanTaoMoi++;
        }

        if (soTaiKhoanTaoMoi > 0 || soTaiKhoanCapNhat > 0)
        {
            context.SaveChanges();
        }

        return new KhoiTaoTaiKhoanMacDinhResult(soVaiTroTaoMoi, soTaiKhoanTaoMoi, soTaiKhoanCapNhat);
    }

    private static Dictionary<RoleEnum, int> DamBaoVaiTroMacDinh(CaPheDbContext context, ref int soVaiTroTaoMoi)
    {
        var ketQua = new Dictionary<RoleEnum, int>();
        var canLuuThayDoi = false;

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
                soVaiTroTaoMoi++;
                canLuuThayDoi = true;
            }

            ketQua[vaiTro] = duLieuVaiTro.ID;
        }

        if (canLuuThayDoi)
        {
            context.SaveChanges();
        }

        foreach (var vaiTro in (RoleEnum[])Enum.GetValues(typeof(RoleEnum)))
        {
            if (ketQua[vaiTro] > 0)
            {
                continue;
            }

            var tenVaiTro = RoleMapper.ToRoleName(vaiTro);
            var duLieuVaiTro = context.VaiTro
                .First(x => x.TenVaiTro == tenVaiTro);
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