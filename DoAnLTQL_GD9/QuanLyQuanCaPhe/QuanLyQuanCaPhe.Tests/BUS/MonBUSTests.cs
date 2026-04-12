using AutoFixture;
using FluentAssertions;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.BUS;

public class MonBUSTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ThemMon_DangKinhDoanhWithZeroPrice_ReturnsValidationFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var roleId = TestDataSeeder.EnsureRole(context, RoleEnum.Admin);
        TestDataSeeder.GrantPermission(context, roleId, PermissionFeatures.Menu, canView: true, canCreate: true, canUpdate: true, canDelete: true);

        var admin = TestDataSeeder.CreateUser(context, "admin_menu_zero", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");

        var sut = new MonBUS();

        var result = sut.ThemMon(new MonDTO
        {
            TenMon = "Mon 0 gia",
            LoaiMonID = loai.ID,
            DonGia = 0,
            TrangThai = 1
        });

        result.ThanhCong.Should().BeFalse();
    }

    [Fact]
    public void CapNhatMon_StaffChangesPrice_ReturnsForbidden()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var roleId = TestDataSeeder.EnsureRole(context, RoleEnum.Staff);
        TestDataSeeder.GrantPermission(context, roleId, PermissionFeatures.Menu, canView: true, canCreate: true, canUpdate: true, canDelete: false);

        var staff = TestDataSeeder.CreateUser(context, "staff_menu_edit", "123", RoleEnum.Staff);
        TestDataSeeder.SetCurrentUser(staff);

        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Espresso", 15000m, 1);

        var sut = new MonBUS();

        var result = sut.CapNhatMon(new MonDTO
        {
            ID = mon.ID,
            TenMon = mon.TenMon,
            LoaiMonID = loai.ID,
            DonGia = 18000m,
            TrangThai = 1
        });

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("sửa giá");
    }

    [Fact]
    public void ThemMon_TrungTenMon_ReturnsValidationFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var roleId = TestDataSeeder.EnsureRole(context, RoleEnum.Admin);
        TestDataSeeder.GrantPermission(context, roleId, PermissionFeatures.Menu, canView: true, canCreate: true, canUpdate: true, canDelete: true);

        var admin = TestDataSeeder.CreateUser(context, "admin_menu_duplicate", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        TestDataSeeder.CreateMon(context, loai.ID, "Espresso", 29000m, 1);

        var sut = new MonBUS();

        var result = sut.ThemMon(new MonDTO
        {
            TenMon = "  espresso  ",
            LoaiMonID = loai.ID,
            DonGia = 32000m,
            TrangThai = 1
        });

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Be("Tên món đã tồn tại.");
    }

    [Fact]
    public void CapNhatMon_DoiTenThanhTenDaTonTai_ReturnsValidationFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var roleId = TestDataSeeder.EnsureRole(context, RoleEnum.Admin);
        TestDataSeeder.GrantPermission(context, roleId, PermissionFeatures.Menu, canView: true, canCreate: true, canUpdate: true, canDelete: true);

        var admin = TestDataSeeder.CreateUser(context, "admin_menu_duplicate_update", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var loai = TestDataSeeder.CreateLoaiMon(context, "Tra");
        var monDaCo = TestDataSeeder.CreateMon(context, loai.ID, "Latte", 28000m, 1);
        var monCanSua = TestDataSeeder.CreateMon(context, loai.ID, "Mocha", 30000m, 1);

        var sut = new MonBUS();

        var result = sut.CapNhatMon(new MonDTO
        {
            ID = monCanSua.ID,
            TenMon = " LATTE ",
            LoaiMonID = loai.ID,
            DonGia = monCanSua.DonGia,
            TrangThai = monCanSua.TrangThai
        });

        monDaCo.ID.Should().BeGreaterThan(0);
        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Be("Tên món đã tồn tại.");
    }

    [Fact]
    public void ThemMon_ValidData_ReturnsSuccess()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var roleId = TestDataSeeder.EnsureRole(context, RoleEnum.Admin);
        TestDataSeeder.GrantPermission(context, roleId, PermissionFeatures.Menu, canView: true, canCreate: true, canUpdate: true, canDelete: true);

        var admin = TestDataSeeder.CreateUser(context, "admin_menu_create", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var loai = TestDataSeeder.CreateLoaiMon(context, "Tra");

        var tenMon = "Mon_" + _fixture.Create<string>()[..6];

        var sut = new MonBUS();

        var result = sut.ThemMon(new MonDTO
        {
            TenMon = tenMon,
            LoaiMonID = loai.ID,
            DonGia = 19000m,
            TrangThai = 1,
            MoTa = "Mon test"
        });

        result.ThanhCong.Should().BeTrue();
        result.MonMoi.Should().NotBeNull();
        result.MonMoi!.ID.Should().BeGreaterThan(0);
    }
}
