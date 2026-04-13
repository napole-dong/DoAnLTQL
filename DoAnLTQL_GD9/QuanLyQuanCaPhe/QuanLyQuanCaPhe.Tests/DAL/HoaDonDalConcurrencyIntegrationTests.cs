using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.DAL;

public class HoaDonDalConcurrencyIntegrationTests
{
    [Fact]
    public async Task CapNhatKhachHangChoHoaDonMo_RaceRowVersion_OnlyOneRequestSucceeds()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "hoadon_dal_race", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var ban = TestDataSeeder.CreateBan(context, "Ban HoaDon DAL Race", 0);
        var hoaDon = TestDataSeeder.CreateHoaDon(context, ban.ID, admin.NhanVienId, trangThai: 0);

        var khachA = new dtaKhachHang
        {
            HoVaTen = "Khach A",
            DienThoai = "0911111111",
            DiaChi = "Dia chi A"
        };

        var khachB = new dtaKhachHang
        {
            HoVaTen = "Khach B",
            DienThoai = "0922222222",
            DiaChi = "Dia chi B"
        };

        context.KhachHang.AddRange(khachA, khachB);
        context.SaveChanges();

        var rowVersion = context.HoaDon
            .AsNoTracking()
            .Where(x => x.ID == hoaDon.ID)
            .Select(x => x.RowVersion)
            .Single()
            .ToArray();

        var sut = new HoaDonDAL();

        using var startGate = new ManualResetEventSlim(false);

        var raceTasks = new[]
        {
            Task.Run(() =>
            {
                startGate.Wait();
                return sut.CapNhatKhachHangChoHoaDonMo(hoaDon.ID, khachA.ID, rowVersion.ToArray());
            }),
            Task.Run(() =>
            {
                startGate.Wait();
                return sut.CapNhatKhachHangChoHoaDonMo(hoaDon.ID, khachB.ID, rowVersion.ToArray());
            })
        };

        startGate.Set();
        var raceResults = await Task.WhenAll(raceTasks);

        raceResults.Count(x => x.ThanhCong).Should().Be(1);
        raceResults.Count(x => !x.ThanhCong).Should().Be(1);
        raceResults.Single(x => !x.ThanhCong).ThongBao.Should().Contain("Vui lòng tải lại");

        using var verifyContext = scope.CreateContext();
        var hoaDonSauCapNhat = verifyContext.HoaDon
            .AsNoTracking()
            .Single(x => x.ID == hoaDon.ID);

        new[] { khachA.ID, khachB.ID }.Should().Contain(hoaDonSauCapNhat.KhachHangID.GetValueOrDefault());

        var khachThang = verifyContext.KhachHang
            .AsNoTracking()
            .Single(x => x.ID == hoaDonSauCapNhat.KhachHangID);

        hoaDonSauCapNhat.CustomerName.Should().Be(khachThang.HoVaTen);

        verifyContext.AuditLog
            .Count(x => x.Action == AuditActions.UpdateInvoice
                && x.EntityName == "Invoice"
                && x.EntityId == hoaDon.ID.ToString())
            .Should()
            .Be(1);
    }
}
