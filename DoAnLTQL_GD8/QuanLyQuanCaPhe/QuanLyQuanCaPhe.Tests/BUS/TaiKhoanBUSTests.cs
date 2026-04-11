using FluentAssertions;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.Tests.BUS;

public class TaiKhoanBUSTests
{
    [Fact]
    public void DeleteUser_AdminDeletesStaff_SetsAccountInactive()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "admin_delete", "123", RoleEnum.Admin);
        var staff = TestDataSeeder.CreateUser(context, "staff_delete", "123", RoleEnum.Staff);
        TestDataSeeder.SetCurrentUser(admin);

        var sut = new TaiKhoanBUS();

        var result = sut.DeleteUser(staff.UserId);

        result.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        verifyContext.User.First(x => x.ID == staff.UserId).HoatDong.Should().BeFalse();
    }

    [Fact]
    public void DeleteUser_AdminCannotDeleteAdminAccount_ReturnsFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var currentAdmin = TestDataSeeder.CreateUser(context, "admin_current", "123", RoleEnum.Admin);
        var targetAdmin = TestDataSeeder.CreateUser(context, "admin_target", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(currentAdmin);

        var sut = new TaiKhoanBUS();

        var result = sut.DeleteUser(targetAdmin.UserId);

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("Admin");
    }

    [Fact]
    public void DeleteUser_CannotDeleteCurrentLoggedInAccount_ReturnsFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "admin_self", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var sut = new TaiKhoanBUS();

        var result = sut.DeleteUser(admin.UserId);

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("đang đăng nhập");
    }
}
