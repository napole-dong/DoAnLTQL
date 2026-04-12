using AutoFixture;
using FluentAssertions;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.BUS;

public class DangNhapBUSTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void DangNhap_ValidCredentials_ReturnsSuccessAndUserInfo()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var username = _fixture.Create<string>()[..8] + "_login";
        var password = "123";
        TestDataSeeder.CreateUser(context, username, password, RoleEnum.Admin, isActive: true);

        var sut = new DangNhapBUS();

        var result = sut.DangNhap(username, password);

        result.ThanhCong.Should().BeTrue();
        result.ThongTinDangNhap.Should().NotBeNull();
        result.ThongTinDangNhap!.TenDangNhap.Should().Be(username);
    }

    [Fact]
    public void DangNhap_DisabledUser_ReturnsLockedMessage()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var username = "locked_user";
        TestDataSeeder.CreateUser(context, username, "123", RoleEnum.Staff, isActive: false);

        var sut = new DangNhapBUS();

        var result = sut.DangNhap(username, "123");

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("khóa", Exactly.Once());
    }

    [Fact]
    public void DangNhap_WrongPassword_ReturnsFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var username = "wrong_pass_user";
        TestDataSeeder.CreateUser(context, username, "123", RoleEnum.Staff, isActive: true);

        var sut = new DangNhapBUS();

        var result = sut.DangNhap(username, "wrong-password");

        result.ThanhCong.Should().BeFalse();
        result.TruongLoi.Should().Be("MatKhau");
    }

    [Fact]
    public void DangNhap_UnknownUsername_ReturnsFailure()
    {
        using var scope = new SqliteTestScope();

        var sut = new DangNhapBUS();

        var result = sut.DangNhap("not_found", "123");

        result.ThanhCong.Should().BeFalse();
        result.TruongLoi.Should().Be("TenDangNhap");
    }
}
