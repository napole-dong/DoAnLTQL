using FluentAssertions;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.DAL;

public class NguyenLieuDalTransactionIntegrationTests
{
    [Fact]
    public void NhapKho_ValidInput_PersistsReceiptAndUpdatesStock()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Sua tx", soLuongTon: 10m, giaNhapGanNhat: 5000m);

        var sut = new NguyenLieuDAL();
        var ketQua = sut.NhapKho(nguyenLieu.ID, soLuongNhap: 5m, giaNhap: 7000m, ghiChu: "Nhap test");

        ketQua.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        var nguyenLieuSauNhap = verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID);

        nguyenLieuSauNhap.SoLuongTon.Should().Be(15m);
        nguyenLieuSauNhap.GiaNhapGanNhat.Should().Be(7000m);

        var phieuNhap = verifyContext.PhieuNhapKho.Single(x => x.NguyenLieuID == nguyenLieu.ID);
        phieuNhap.SoLuongNhap.Should().Be(5m);
        phieuNhap.GiaNhap.Should().Be(7000m);
    }

    [Fact]
    public void NhapKho_IngredientNotFound_RollsBackAndCreatesNoReceipt()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Du phong", soLuongTon: 20m, giaNhapGanNhat: 3000m);

        var sut = new NguyenLieuDAL();
        var ketQua = sut.NhapKho(maNguyenLieu: 999999, soLuongNhap: 4m, giaNhap: 6000m, ghiChu: null);

        ketQua.ThanhCong.Should().BeFalse();
        ketQua.ThongBao.Should().Contain("Không tìm thấy nguyên liệu");

        using var verifyContext = scope.CreateContext();
        verifyContext.PhieuNhapKho.Should().BeEmpty();
        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID).SoLuongTon.Should().Be(20m);
    }
}
