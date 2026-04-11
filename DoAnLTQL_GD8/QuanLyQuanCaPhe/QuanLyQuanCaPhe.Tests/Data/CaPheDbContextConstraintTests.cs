using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.Tests.Data;

public class CaPheDbContextConstraintTests
{
    [Fact]
    public void Users_UsernameMustBeUnique()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var roleId = TestDataSeeder.EnsureRole(context, RoleEnum.Staff);

        var nv1 = new dtaNhanVien { HoVaTen = "NV1" };
        var nv2 = new dtaNhanVien { HoVaTen = "NV2" };
        context.NhanVien.AddRange(nv1, nv2);
        context.SaveChanges();

        context.User.Add(new dtaUser
        {
            NhanVienID = nv1.ID,
            VaiTroID = roleId,
            TenDangNhap = "duplicate_user",
            MatKhau = MatKhauService.BamMatKhau("123"),
            HoatDong = true
        });

        context.User.Add(new dtaUser
        {
            NhanVienID = nv2.ID,
            VaiTroID = roleId,
            TenDangNhap = "duplicate_user",
            MatKhau = MatKhauService.BamMatKhau("123"),
            HoatDong = true
        });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void HoaDon_OnlyOneOpenInvoicePerTable_IsEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var user = TestDataSeeder.CreateUser(context, "invoice_unique", "123", RoleEnum.Admin);
        var ban = TestDataSeeder.CreateBan(context, "Ban 11", trangThai: 0);

        context.HoaDon.Add(new dtaHoadon
        {
            BanID = ban.ID,
            NhanVienID = user.NhanVienId,
            CustomerName = "Khach le",
            NgayLap = DateTime.Now,
            TrangThai = 0,
            TongTien = 0m
        });

        context.HoaDon.Add(new dtaHoadon
        {
            BanID = ban.ID,
            NhanVienID = user.NhanVienId,
            CustomerName = "Khach le",
            NgayLap = DateTime.Now,
            TrangThai = 0,
            TongTien = 0m
        });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void HoaDonChiTiet_InvalidForeignKey_Throws()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var user = TestDataSeeder.CreateUser(context, "fk_user", "123", RoleEnum.Admin);
        var ban = TestDataSeeder.CreateBan(context, "Ban FK", 0);
        var invoice = TestDataSeeder.CreateHoaDon(context, ban.ID, user.NhanVienId);

        context.HoaDon_ChiTiet.Add(new dtHoaDon_ChiTiet
        {
            HoaDonID = invoice.ID,
            MonID = 999999,
            SoLuongBan = 1,
            DonGiaBan = 10000m,
            ThanhTien = 10000m
        });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void HoaDonChiTiet_SoLuongBanMustBeGreaterThanZero_IsEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var user = TestDataSeeder.CreateUser(context, "ck_user", "123", RoleEnum.Admin);
        var ban = TestDataSeeder.CreateBan(context, "Ban CK", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Loai CK");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Mon CK", 10000m, 1);
        var invoice = TestDataSeeder.CreateHoaDon(context, ban.ID, user.NhanVienId);

        context.HoaDon_ChiTiet.Add(new dtHoaDon_ChiTiet
        {
            HoaDonID = invoice.ID,
            MonID = mon.ID,
            SoLuongBan = 0,
            DonGiaBan = 10000m,
            ThanhTien = 0m
        });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void Ban_TenBanIsRequired_IsEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        context.Ban.Add(new dtaBan
        {
            TenBan = null!,
            TrangThai = 0
        });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }
}
