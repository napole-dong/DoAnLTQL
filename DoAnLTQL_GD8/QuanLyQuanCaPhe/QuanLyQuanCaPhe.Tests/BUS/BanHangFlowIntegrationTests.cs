using FluentAssertions;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.BUS;

public class BanHangFlowIntegrationTests
{
    [Fact]
    public void Login_CreateInvoice_AddItem_Checkout_FullFlowSuccess()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var staffRoleId = TestDataSeeder.EnsureRole(context, RoleEnum.Staff);
        TestDataSeeder.GrantPermission(context, staffRoleId, PermissionFeatures.BanHang, canView: true, canCreate: true, canUpdate: true, canDelete: false);

        var user = TestDataSeeder.CreateUser(context, "staff_flow", "123", RoleEnum.Staff, isActive: true);

        var ban = TestDataSeeder.CreateBan(context, "Ban Flow", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Flow Latte", 15000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Sua tuoi", soLuongTon: 100m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 1m);

        var dangNhapBus = new DangNhapBUS();
        var login = dangNhapBus.DangNhap("staff_flow", "123");
        login.ThanhCong.Should().BeTrue();
        login.ThongTinDangNhap.Should().NotBeNull();

        NguoiDungHienTaiService.DatNguoiDungDangNhap(login.ThongTinDangNhap!);

        var banHangBus = new BanHangBUS();

        var addTemp = banHangBus.ThemMonVaoGioTam(ban.ID, new MonDTO
        {
            ID = mon.ID,
            TenMon = mon.TenMon,
            DonGia = mon.DonGia,
            LoaiMonID = loai.ID,
            TrangThai = mon.TrangThai
        });

        var save = banHangBus.LuuMonChoGoi(ban.ID);
        var checkout = banHangBus.ThanhToanHoaDon(ban.ID);

        addTemp.ThanhCong.Should().BeTrue();
        save.ThanhCong.Should().BeTrue();
        checkout.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        var invoice = verifyContext.HoaDon
            .OrderByDescending(x => x.ID)
            .First(x => x.BanID == ban.ID);

        invoice.TrangThai.Should().Be(1);
        verifyContext.HoaDon_ChiTiet.Count(x => x.HoaDonID == invoice.ID).Should().BeGreaterThan(0);
        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID).SoLuongTon.Should().Be(99m);
    }
}
