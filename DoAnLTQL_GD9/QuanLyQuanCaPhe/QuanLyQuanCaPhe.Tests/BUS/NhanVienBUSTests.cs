using FluentAssertions;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.BUS;

public class NhanVienBUSTests
{
    [Fact]
    public void XoaNhanVien_DeleteCurrentLoggedInEmployee_ReturnsForbidden()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var adminRoleId = TestDataSeeder.EnsureRole(context, RoleEnum.Admin);
        TestDataSeeder.GrantPermission(context, adminRoleId, PermissionFeatures.NhanVien, canView: true, canCreate: true, canUpdate: true, canDelete: true);

        var admin = TestDataSeeder.CreateUser(context, "admin_nhanvien_self", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var sut = new NhanVienBUS();

        var result = sut.XoaNhanVien(admin.NhanVienId, softDelete: true);

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("đăng nhập");
    }

    [Fact]
    public void XoaNhanVien_TargetAdminAccount_ReturnsForbidden()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var adminRoleId = TestDataSeeder.EnsureRole(context, RoleEnum.Admin);
        TestDataSeeder.GrantPermission(context, adminRoleId, PermissionFeatures.NhanVien, canView: true, canCreate: true, canUpdate: true, canDelete: true);

        var currentAdmin = TestDataSeeder.CreateUser(context, "admin_nhanvien_current", "123", RoleEnum.Admin);
        var targetAdmin = TestDataSeeder.CreateUser(context, "admin_nhanvien_target", "123", RoleEnum.Admin);

        TestDataSeeder.SetCurrentUser(currentAdmin);

        var sut = new NhanVienBUS();

        var result = sut.XoaNhanVien(targetAdmin.NhanVienId, softDelete: true);

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("Admin");
    }
}
