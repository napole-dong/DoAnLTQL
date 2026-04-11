using FluentAssertions;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.BUS;

public class PermissionBUSTests
{
    [Fact]
    public void CheckPermission_NoSession_ReturnsFalse()
    {
        using var scope = new SqliteTestScope();
        NguoiDungHienTaiService.XoaNguoiDungDangNhap();

        var sut = new PermissionBUS();

        var result = sut.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);

        result.Should().BeFalse();
    }

    [Fact]
    public void CheckPermission_GrantedPermission_ReturnsTrue()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var roleId = TestDataSeeder.EnsureRole(context, RoleEnum.Manager);
        TestDataSeeder.GrantPermission(context, roleId, PermissionFeatures.HoaDon, canView: true, canCreate: false, canUpdate: false, canDelete: false);

        var manager = TestDataSeeder.CreateUser(context, "manager_perm", "123", RoleEnum.Manager);
        TestDataSeeder.SetCurrentUser(manager);

        var sut = new PermissionBUS();

        var result = sut.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);

        result.Should().BeTrue();
    }

    [Fact]
    public void CheckPermission_UnknownFeature_ReturnsFalse()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "admin_perm", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var sut = new PermissionBUS();

        var result = sut.CheckPermission("KhongTonTai", PermissionActions.View);

        result.Should().BeFalse();
    }

    [Fact]
    public void CanDeleteInvoice_AdminAndManager_ReturnsExpectedResult()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "admin_invoice", "123", RoleEnum.Admin);
        var manager = TestDataSeeder.CreateUser(context, "manager_invoice", "123", RoleEnum.Manager);
        var sut = new PermissionBUS();

        TestDataSeeder.SetCurrentUser(admin);
        var adminCanDelete = sut.CanDeleteInvoice();

        TestDataSeeder.SetCurrentUser(manager);
        var managerCanDelete = sut.CanDeleteInvoice();

        adminCanDelete.Should().BeTrue();
        managerCanDelete.Should().BeFalse();
    }
}
