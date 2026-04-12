using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.DAL;

public class LoaiMonDalDeleteIntegrationTests
{
    [Fact]
    public void XoaLoai_KhiLoaiDangCoMon_SeNgungBanMonVaSoftDeleteLoai()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var loai = TestDataSeeder.CreateLoaiMon(context, "Loai xoa");
        var mon = TestDataSeeder.CreateMon(context, loai.ID, "Mon xoa", 25000m, trangThai: 1);

        var sut = new LoaiMonDAL();
        var daXoa = sut.XoaLoai(loai.ID);

        daXoa.Should().BeTrue();

        using var verifyContext = scope.CreateContext();

        verifyContext.LoaiMon.Any(x => x.ID == loai.ID).Should().BeFalse();

        var loaiDaXoa = verifyContext.LoaiMon
            .IgnoreQueryFilters()
            .Single(x => x.ID == loai.ID);

        loaiDaXoa.IsDeleted.Should().BeTrue();
        loaiDaXoa.DeletedAt.Should().NotBeNull();

        var monSauKhiXoaLoai = verifyContext.Mon
            .IgnoreQueryFilters()
            .Single(x => x.ID == mon.ID);

        monSauKhiXoaLoai.TrangThai.Should().Be(0);
        monSauKhiXoaLoai.DonGia.Should().Be(0m);
        monSauKhiXoaLoai.TrangThaiTextLegacy.Should().Be("Ngừng bán");
    }

    [Fact]
    public void XoaLoai_KhiLoaiKhongTonTai_SeTraVeFalse()
    {
        using var scope = new SqliteTestScope();

        var sut = new LoaiMonDAL();
        var daXoa = sut.XoaLoai(999999);

        daXoa.Should().BeFalse();
    }
}
