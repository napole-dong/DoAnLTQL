using FluentAssertions;
using Moq;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Services.SoftDelete;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.Services;

public class SoftDeleteServiceTests
{
    [Fact]
    public void SoftDelete_WhenRepositoryReturnsTrue_ReturnsTrue()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var entity = new dtaMon
        {
            LoaiMonID = 1,
            TenMon = "Test",
            DonGia = 1,
            TrangThai = 1,
            TrangThaiTextLegacy = "Dang kinh doanh"
        };

        var repo = new Mock<ISoftDeleteRepository>();
        repo
            .Setup(x => x.Remove(context, entity))
            .Returns(true);

        var sut = new SoftDeleteService(repo.Object);

        var result = sut.SoftDelete(context, entity);

        result.Should().BeTrue();
        repo.Verify(x => x.Remove(context, entity), Times.Once);
    }

    [Fact]
    public void Restore_ShouldCallRepositoryRestore()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var entity = new dtaKhachHang { HoVaTen = "Khach" };

        var repo = new Mock<ISoftDeleteRepository>();
        var sut = new SoftDeleteService(repo.Object);

        sut.Restore(context, entity);

        repo.Verify(x => x.Restore(context, entity), Times.Once);
    }

    [Fact]
    public void HardDelete_ShouldCallRepositoryHardDelete()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var entity = new dtaNhanVien { HoVaTen = "Nhan Vien" };

        var repo = new Mock<ISoftDeleteRepository>();
        var sut = new SoftDeleteService(repo.Object);

        sut.HardDelete(context, entity);

        repo.Verify(x => x.HardDelete(context, entity), Times.Once);
    }
}
