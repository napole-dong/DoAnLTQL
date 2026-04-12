using FluentAssertions;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.BUS;

public class HoaDonBUSTests
{
    [Fact]
    public void ThemHoaDon_ValidData_ReturnsSuccessAndInvoiceId()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var staffRoleId = TestDataSeeder.EnsureRole(context, RoleEnum.Staff);
        TestDataSeeder.GrantPermission(context, staffRoleId, PermissionFeatures.HoaDon, canView: true, canCreate: true, canUpdate: true, canDelete: false);

        var staff = TestDataSeeder.CreateUser(context, "staff_invoice_create", "123", RoleEnum.Staff);
        TestDataSeeder.SetCurrentUser(staff);

        var ban = TestDataSeeder.CreateBan(context, "Ban Invoice", 0);

        var sut = new HoaDonBUS();

        var (result, hoaDonId) = sut.ThemHoaDon(new HoaDonSaveRequestDTO
        {
            BanID = ban.ID,
            NgayLap = DateTime.Now
        });

        result.ThanhCong.Should().BeTrue();
        hoaDonId.Should().BeGreaterThan(0);
    }

    [Fact]
    public void XacNhanThuTien_EmptyInvoice_ReturnsFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var staffRoleId = TestDataSeeder.EnsureRole(context, RoleEnum.Staff);
        TestDataSeeder.GrantPermission(context, staffRoleId, PermissionFeatures.HoaDon, canView: true, canCreate: true, canUpdate: true, canDelete: false);

        var staff = TestDataSeeder.CreateUser(context, "staff_invoice_pay", "123", RoleEnum.Staff);
        TestDataSeeder.SetCurrentUser(staff);

        var ban = TestDataSeeder.CreateBan(context, "Ban Empty Invoice", 1);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, staff.NhanVienId, trangThai: 0, tongTien: 0m);

        var sut = new HoaDonBUS();

        var result = sut.XacNhanThuTien(hoaDon.ID, tienKhachDua: 100000m, rowVersion: hoaDon.RowVersion);

        result.ThanhCong.Should().BeFalse();
    }

    [Fact]
    public void HuyHoaDon_NonAdmin_ReturnsFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var managerRoleId = TestDataSeeder.EnsureRole(context, RoleEnum.Manager);
        TestDataSeeder.GrantPermission(context, managerRoleId, PermissionFeatures.HoaDon, canView: true, canCreate: true, canUpdate: true, canDelete: true);

        var manager = TestDataSeeder.CreateUser(context, "manager_invoice_delete", "123", RoleEnum.Manager);
        TestDataSeeder.SetCurrentUser(manager);

        var ban = TestDataSeeder.CreateBan(context, "Ban Manager", 1);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, manager.NhanVienId, trangThai: 0, tongTien: 10000m);

        var sut = new HoaDonBUS();

        var result = sut.HuyHoaDon(hoaDon.ID, hoaDon.RowVersion);

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("Admin");
    }

    [Fact]
    public void ThemMonVaoHoaDon_NegativeQuantity_ReturnsValidationFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "admin_invoice_item", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var sut = new HoaDonBUS();

        var result = sut.ThemMonVaoHoaDon(hoaDonId: 1, monId: 1, soLuong: -1);

        result.ThanhCong.Should().BeFalse();
    }
}
