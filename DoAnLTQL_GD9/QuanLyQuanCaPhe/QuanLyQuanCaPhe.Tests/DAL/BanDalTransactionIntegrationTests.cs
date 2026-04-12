using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.DAL;

public class BanDalTransactionIntegrationTests
{
    [Fact]
    public void ChuyenHoacGopBan_ChuyenBan_Success_MovesOpenInvoiceAndUpdatesTableState()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "ban_tx_transfer", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var banNguon = TestDataSeeder.CreateBan(context, "Ban Nguon TX", trangThai: 1);
        var banDich = TestDataSeeder.CreateBan(context, "Ban Dich TX", trangThai: 0);
        var hoaDonNguon = TestDataSeeder.CreateHoaDon(context, banNguon.ID, admin.NhanVienId, trangThai: (int)HoaDonTrangThai.Draft);

        var sut = new BanDAL();
        var ketQua = sut.ChuyenHoacGopBan(new BanChuyenGopRequestDTO
        {
            BanNguonId = banNguon.ID,
            BanDichId = banDich.ID,
            LaChuyenBan = true
        });

        ketQua.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon.First(x => x.ID == hoaDonNguon.ID).BanID.Should().Be(banDich.ID);
        verifyContext.Ban.First(x => x.ID == banNguon.ID).TrangThai.Should().Be(0);
        verifyContext.Ban.First(x => x.ID == banDich.ID).TrangThai.Should().Be(1);
    }

    [Fact]
    public void ChuyenHoacGopBan_ChuyenBan_ToBusyTable_RollsBackAndKeepsOriginalData()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "ban_tx_busy", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var banNguon = TestDataSeeder.CreateBan(context, "Ban Nguon Busy", trangThai: 1);
        var banDich = TestDataSeeder.CreateBan(context, "Ban Dich Busy", trangThai: 1);

        var hoaDonNguon = TestDataSeeder.CreateHoaDon(context, banNguon.ID, admin.NhanVienId, trangThai: (int)HoaDonTrangThai.Draft);
        _ = TestDataSeeder.CreateHoaDon(context, banDich.ID, admin.NhanVienId, trangThai: (int)HoaDonTrangThai.Draft);

        var sut = new BanDAL();
        var ketQua = sut.ChuyenHoacGopBan(new BanChuyenGopRequestDTO
        {
            BanNguonId = banNguon.ID,
            BanDichId = banDich.ID,
            LaChuyenBan = true
        });

        ketQua.ThanhCong.Should().BeFalse();

        using var verifyContext = scope.CreateContext();
        verifyContext.HoaDon.First(x => x.ID == hoaDonNguon.ID).BanID.Should().Be(banNguon.ID);
        verifyContext.HoaDon.Count(x => x.TrangThai == (int)HoaDonTrangThai.Draft).Should().Be(2);
        verifyContext.Ban.First(x => x.ID == banNguon.ID).TrangThai.Should().Be(1);
        verifyContext.Ban.First(x => x.ID == banDich.ID).TrangThai.Should().Be(1);
    }

    [Fact]
    public void ChuyenHoacGopBan_GopBan_Success_MergesLineItemsAndDeletesSourceInvoice()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var admin = TestDataSeeder.CreateUser(context, "ban_tx_merge", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(admin);

        var banNguon = TestDataSeeder.CreateBan(context, "Ban Nguon Merge", trangThai: 1);
        var banDich = TestDataSeeder.CreateBan(context, "Ban Dich Merge", trangThai: 1);

        var loai = TestDataSeeder.CreateLoaiMon(context, "Cafe");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Merge Latte", 20000m, 1);

        var hoaDonNguon = TestDataSeeder.CreateHoaDon(context, banNguon.ID, admin.NhanVienId, trangThai: (int)HoaDonTrangThai.Draft);
        var hoaDonDich = TestDataSeeder.CreateHoaDon(context, banDich.ID, admin.NhanVienId, trangThai: (int)HoaDonTrangThai.Draft);

        TestDataSeeder.CreateHoaDonChiTiet(context, hoaDonNguon.ID, mon.ID, soLuong: 2, donGia: 20000m);
        TestDataSeeder.CreateHoaDonChiTiet(context, hoaDonDich.ID, mon.ID, soLuong: 1, donGia: 20000m);

        var sut = new BanDAL();
        var ketQua = sut.ChuyenHoacGopBan(new BanChuyenGopRequestDTO
        {
            BanNguonId = banNguon.ID,
            BanDichId = banDich.ID,
            LaChuyenBan = false
        });

        ketQua.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();

        verifyContext.HoaDon.FirstOrDefault(x => x.ID == hoaDonNguon.ID).Should().BeNull();

        var hoaDonDichSauGop = verifyContext.HoaDon
            .Include(x => x.HoaDon_ChiTiet)
            .First(x => x.ID == hoaDonDich.ID);

        var chiTietDaGop = hoaDonDichSauGop.HoaDon_ChiTiet
            .Single(x => x.MonID == mon.ID && x.GhiChu == null);

        chiTietDaGop.SoLuongBan.Should().Be(3);
        chiTietDaGop.ThanhTien.Should().Be(60000m);
        hoaDonDichSauGop.TongTien.Should().Be(60000m);
        hoaDonDichSauGop.GhiChuHoaDon.Should().Contain("Gop ban");

        verifyContext.Ban.First(x => x.ID == banNguon.ID).TrangThai.Should().Be(0);
        verifyContext.Ban.First(x => x.ID == banDich.ID).TrangThai.Should().Be(1);
    }
}
