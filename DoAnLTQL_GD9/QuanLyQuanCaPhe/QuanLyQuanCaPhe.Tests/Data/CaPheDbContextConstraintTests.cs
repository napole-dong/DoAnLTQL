using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
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

    [Fact]
    public void Ban_TenBanMustBeUnique_IsEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        context.Ban.Add(new dtaBan { TenBan = "Ban Unique", TrangThai = 0 });
        context.Ban.Add(new dtaBan { TenBan = "Ban Unique", TrangThai = 1 });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void LoaiMon_ActiveNameMustBeUnique_IsEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        context.LoaiMon.Add(new dtaLoaiMon { TenLoai = "Loai Unique" });
        context.LoaiMon.Add(new dtaLoaiMon { TenLoai = "Loai Unique" });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void KhachHang_ActivePhoneMustBeUnique_IsEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        context.KhachHang.Add(new dtaKhachHang { HoVaTen = "Khach 1", DienThoai = "0909000001" });
        context.KhachHang.Add(new dtaKhachHang { HoVaTen = "Khach 2", DienThoai = "0909000001" });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void Mon_DonGiaOutOfRange_IsEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var loai = TestDataSeeder.CreateLoaiMon(context, "Loai Gia");

        context.Mon.Add(new dtaMon
        {
            LoaiMonID = loai.ID,
            TenMon = "Mon Gia Loi",
            DonGia = -1m,
            TrangThai = 1,
            TrangThaiTextLegacy = "Dang kinh doanh"
        });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void HoaDon_TongTienOutOfRange_IsEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var user = TestDataSeeder.CreateUser(context, "invoice_range", "123", RoleEnum.Admin);
        var ban = TestDataSeeder.CreateBan(context, "Ban Range", 0);

        context.HoaDon.Add(new dtaHoadon
        {
            BanID = ban.ID,
            NhanVienID = user.NhanVienId,
            CustomerName = "Khach le",
            NgayLap = DateTime.Now,
            TrangThai = (int)HoaDonTrangThai.Draft,
            TongTien = -1m
        });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void NguyenLieu_NonNegativeFields_AreEnforced()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        context.NguyenLieu.Add(new dtaNguyenLieu
        {
            TenNguyenLieu = "Nguyen lieu loi",
            DonViTinh = "kg",
            SoLuongTon = -1m,
            MucCanhBao = 1m,
            GiaNhapGanNhat = -10m,
            TrangThai = 1,
            TrangThaiTextLegacy = "Dang su dung"
        });

        Action act = () => context.SaveChanges();

        act.Should().Throw<DbUpdateException>();
    }
}
