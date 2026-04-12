using FluentAssertions;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.BUS;

public class OrderServiceTests
{
    [Fact]
    public void AddItemToOrder_ValidData_DeductsInventoryAndRecalculatesTotal()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_admin", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Order", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Latte", 20000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Sua", soLuongTon: 100m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 2m);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0);

        var sut = new OrderService();

        var result = sut.AddItemToOrder(hoaDon.ID, mon.ID, 3);

        result.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        var invoice = verifyContext.HoaDon
            .Where(x => x.ID == hoaDon.ID)
            .Select(x => new
            {
                x.TongTien,
                x.TrangThai,
                ChiTiet = x.HoaDon_ChiTiet.Select(c => new { c.MonID, c.SoLuongBan, c.ThanhTien }).ToList()
            })
            .First();

        invoice.TongTien.Should().Be(60000m);
        invoice.ChiTiet.Should().ContainSingle();
        invoice.ChiTiet[0].SoLuongBan.Should().Be(3);
        invoice.ChiTiet[0].ThanhTien.Should().Be(60000m);

        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID).SoLuongTon.Should().Be(94m);
        verifyContext.PhieuXuatKho.Count().Should().Be(1);
    }

    [Fact]
    public void AddItemToOrder_InsufficientStock_ReturnsFailureAndDoesNotChangeInventory()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_stock", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Stock", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Espresso", 30000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Hat", soLuongTon: 5m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 3m);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0);

        var sut = new OrderService();

        var result = sut.AddItemToOrder(hoaDon.ID, mon.ID, 2);

        result.ThanhCong.Should().BeFalse();

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon_ChiTiet.Count(x => x.HoaDonID == hoaDon.ID).Should().Be(0);
        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID).SoLuongTon.Should().Be(5m);
    }

    [Fact]
    public void CancelOrder_AfterAddingItems_RestoresInventoryAndMarksCancelled()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_cancel", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Cancel", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Cappuccino", 25000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Cream", soLuongTon: 100m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 4m);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0);

        var sut = new OrderService();
        var addResult = sut.AddItemToOrder(hoaDon.ID, mon.ID, 2);
        addResult.ThanhCong.Should().BeTrue();

        var cancelResult = sut.CancelOrder(hoaDon.ID);

        cancelResult.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon.First(x => x.ID == hoaDon.ID).TrangThai.Should().Be((int)HoaDonTrangThai.DaHuy);
        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID).SoLuongTon.Should().Be(100m);
        verifyContext.PhieuNhapKho.Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public void Checkout_EmptyInvoice_ReturnsFailure()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_checkout_empty", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Empty", 0);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0, tongTien: 0m);

        var sut = new OrderService();

        var result = sut.Checkout(hoaDon.ID);

        result.ThanhCong.Should().BeFalse();

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon.First(x => x.ID == hoaDon.ID).TrangThai.Should().Be((int)HoaDonTrangThai.Draft);
        verifyContext.Ban.First(x => x.ID == ban.ID).TrangThai.Should().Be(0);
    }

    [Fact]
    public void Checkout_WithItems_MarksPaid_AndKeepsTableState()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_checkout", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Checkout", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Mocha", 22000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Cacao", soLuongTon: 100m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 1m);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0);

        var sut = new OrderService();
        sut.AddItemToOrder(hoaDon.ID, mon.ID, 1).ThanhCong.Should().BeTrue();

        var result = sut.Checkout(hoaDon.ID);

        result.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon.First(x => x.ID == hoaDon.ID).TrangThai.Should().Be((int)HoaDonTrangThai.Paid);
        verifyContext.Ban.First(x => x.ID == ban.ID).TrangThai.Should().Be(1);
    }

    [Fact]
    public void AddItemToOrder_StaleRowVersion_ReturnsConcurrencyConflict()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_concurrency", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Concurrency", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Americano", 18000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Nuoc", soLuongTon: 100m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 1m);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0);

        var sut = new OrderService();

        var result = sut.AddItemToOrder(hoaDon.ID, mon.ID, 1, expectedRowVersion: new byte[] { 9, 9, 9 });

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("Vui lòng tải lại");
    }

    [Fact]
    public void Checkout_StaleRowVersion_RollsBackAndKeepsState()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_checkout_stale", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Checkout Stale", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Cold Brew", 30000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Da", soLuongTon: 100m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 1m);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0);

        var sut = new OrderService();
        sut.AddItemToOrder(hoaDon.ID, mon.ID, 1).ThanhCong.Should().BeTrue();

        var result = sut.Checkout(hoaDon.ID, expectedRowVersion: new byte[] { 1, 2, 3, 4 });

        result.ThanhCong.Should().BeFalse();
        result.ThongBao.Should().Contain("Vui lòng tải lại");

        using var verifyContext = scope.CreateContext();
        var invoice = verifyContext.HoaDon.First(x => x.ID == hoaDon.ID);

        invoice.TrangThai.Should().Be((int)HoaDonTrangThai.Draft);
        verifyContext.Ban.First(x => x.ID == ban.ID).TrangThai.Should().Be(1);
        verifyContext.AuditLog
            .Where(x => x.EntityName == "Invoice" && x.EntityId == hoaDon.ID.ToString() && x.Action == AuditActions.PayInvoice)
            .Should()
            .BeEmpty();
    }
}
