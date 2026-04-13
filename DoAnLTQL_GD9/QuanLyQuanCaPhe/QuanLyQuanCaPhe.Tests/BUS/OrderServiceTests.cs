using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
    public void CancelOrder_LegacyInvoiceWithoutRecipe_StillVoidsSuccessfully()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_cancel_legacy", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Cancel Legacy", 1);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe Legacy");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Legacy Drink", 25000m, 1);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: (int)HoaDonTrangThai.Open, tongTien: 50000m);

        TestDataSeeder.CreateHoaDonChiTiet(context, hoaDon.ID, mon.ID, soLuong: 2, donGia: 25000m);

        var sut = new OrderService();

        var cancelResult = sut.CancelOrder(hoaDon.ID);

        cancelResult.ThanhCong.Should().BeTrue();
        cancelResult.ThongBao.Should().Contain("Hủy hóa đơn thành công");

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon.First(x => x.ID == hoaDon.ID).TrangThai.Should().Be((int)HoaDonTrangThai.Voided);
        verifyContext.Ban.First(x => x.ID == ban.ID).TrangThai.Should().Be(0);
        verifyContext.PhieuNhapKho.Count().Should().Be(0);
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
    public void AddItemsByTableAtomic_InsufficientStock_RollsBackWithoutCreatingInvoice()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_atomic_rollback", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Atomic Rollback", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe Atomic Rollback");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Atomic Drink", 30000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Atomic NL", soLuongTon: 2m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 3m);

        var sut = new OrderService();

        var ketQua = sut.AddItemsByTableAtomic(
            ban.ID,
            new[]
            {
                new BanHangThemMonDTO
                {
                    MonID = mon.ID,
                    SoLuong = 1
                }
            });

        ketQua.ThanhCong.Should().BeFalse();

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon.Any(x => x.BanID == ban.ID).Should().BeFalse();
        verifyContext.Ban.First(x => x.ID == ban.ID).TrangThai.Should().Be(0);
        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID).SoLuongTon.Should().Be(2m);
        verifyContext.PhieuXuatKho.Count().Should().Be(0);
        verifyContext.AuditLog.Any(x => x.Action == AuditActions.AddItem).Should().BeFalse();
    }

    [Fact]
    public void AddItemsByTableAtomic_DuplicateItems_AggregatesQuantityAndWritesAudit()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_atomic_merge", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Atomic Merge", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe Atomic Merge");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Atomic Latte", 20000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Atomic Milk", soLuongTon: 100m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 2m);

        var sut = new OrderService();

        var ketQua = sut.AddItemsByTableAtomic(
            ban.ID,
            new[]
            {
                new BanHangThemMonDTO { MonID = mon.ID, SoLuong = 1 },
                new BanHangThemMonDTO { MonID = mon.ID, SoLuong = 2 }
            });

        ketQua.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        var hoaDon = verifyContext.HoaDon
            .Include(x => x.HoaDon_ChiTiet)
            .First(x => x.BanID == ban.ID);

        hoaDon.TrangThai.Should().Be((int)HoaDonTrangThai.Draft);
        hoaDon.TongTien.Should().Be(60000m);
        hoaDon.HoaDon_ChiTiet.Should().ContainSingle();
        hoaDon.HoaDon_ChiTiet.First().SoLuongBan.Should().Be(3);

        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID).SoLuongTon.Should().Be(94m);
        verifyContext.AuditLog
            .Any(x => x.Action == AuditActions.AddItem && x.EntityName == "Invoice")
            .Should()
            .BeTrue();
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

    [Fact]
    public async Task Checkout_RaceCondition_OnlyOneRequestSucceeds()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "order_checkout_race", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban Checkout Race", 0);
        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe Race");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Race Drink", 40000m, 1);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Race Nguyen Lieu", soLuongTon: 100m);
        TestDataSeeder.CreateCongThuc(context, mon.ID, nguyenLieu.ID, 1m);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0);

        var sut = new OrderService();
        sut.AddItemToOrder(hoaDon.ID, mon.ID, 1).ThanhCong.Should().BeTrue();

        var rowVersion = context.HoaDon
            .AsNoTracking()
            .Where(x => x.ID == hoaDon.ID)
            .Select(x => x.RowVersion)
            .Single()
            .ToArray();

        using var startGate = new ManualResetEventSlim(false);

        var raceTasks = new[]
        {
            Task.Run(() =>
            {
                startGate.Wait();
                return sut.Checkout(hoaDon.ID, expectedRowVersion: rowVersion.ToArray());
            }),
            Task.Run(() =>
            {
                startGate.Wait();
                return sut.Checkout(hoaDon.ID, expectedRowVersion: rowVersion.ToArray());
            })
        };

        startGate.Set();
        var raceResults = await Task.WhenAll(raceTasks);

        raceResults.Count(x => x.ThanhCong).Should().Be(1);
        raceResults.Count(x => !x.ThanhCong).Should().Be(1);

        var thongBaoThatBai = raceResults.Single(x => !x.ThanhCong).ThongBao ?? string.Empty;
        (thongBaoThatBai.Contains("Vui lòng tải lại", StringComparison.OrdinalIgnoreCase)
            || thongBaoThatBai.Contains("chờ thanh toán", StringComparison.OrdinalIgnoreCase))
            .Should()
            .BeTrue();

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon
            .AsNoTracking()
            .Single(x => x.ID == hoaDon.ID)
            .TrangThai
            .Should()
            .Be((int)HoaDonTrangThai.Paid);

        verifyContext.AuditLog
            .Count(x => x.EntityName == "Invoice" && x.EntityId == hoaDon.ID.ToString() && x.Action == AuditActions.PayInvoice)
            .Should()
            .Be(1);
    }
}
