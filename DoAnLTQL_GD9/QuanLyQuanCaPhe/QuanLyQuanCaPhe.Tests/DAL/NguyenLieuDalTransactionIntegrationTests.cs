using FluentAssertions;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Tests.TestInfrastructure;

namespace QuanLyQuanCaPhe.Tests.DAL;

public class NguyenLieuDalTransactionIntegrationTests
{
    [Fact]
    public void NhapKho_ValidInput_PersistsReceiptAndUpdatesStock()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var user = TestDataSeeder.CreateUser(context, "warehouse_import_ok", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(user);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Sua tx", soLuongTon: 10m, giaNhapGanNhat: 5000m);

        var sut = new NguyenLieuDAL();
        var ketQua = sut.NhapKho(nguyenLieu.ID, soLuongNhap: 5m, giaNhap: 7000m, ghiChu: "Nhap test");

        ketQua.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        var nguyenLieuSauNhap = verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID);

        nguyenLieuSauNhap.SoLuongTon.Should().Be(15m);
        nguyenLieuSauNhap.GiaNhapGanNhat.Should().Be(7000m);

        verifyContext.PhieuNhapKho.Count().Should().Be(1);
        var chiTietPhieuNhap = verifyContext.ChiTietPhieuNhap.Single(x => x.NguyenLieuID == nguyenLieu.ID);
        chiTietPhieuNhap.SoLuong.Should().Be(5m);
        chiTietPhieuNhap.DonGiaNhap.Should().Be(7000m);
    }

    [Fact]
    public void NhapKho_IngredientNotFound_RollsBackAndCreatesNoReceipt()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var user = TestDataSeeder.CreateUser(context, "warehouse_import_notfound", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(user);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Du phong", soLuongTon: 20m, giaNhapGanNhat: 3000m);

        var sut = new NguyenLieuDAL();
        var ketQua = sut.NhapKho(maNguyenLieu: 999999, soLuongNhap: 4m, giaNhap: 6000m, ghiChu: null);

        ketQua.ThanhCong.Should().BeFalse();
        ketQua.ThongBao.Should().Contain("Không tìm thấy nguyên liệu");

        using var verifyContext = scope.CreateContext();
        verifyContext.PhieuNhapKho.Should().BeEmpty();
        verifyContext.ChiTietPhieuNhap.Should().BeEmpty();
        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID).SoLuongTon.Should().Be(20m);
    }

    [Fact]
    public void NhapKhoNhieuNguyenLieu_CreatesSingleMasterWithMultipleDetails()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var user = TestDataSeeder.CreateUser(context, "warehouse_import_batch", "123", RoleEnum.Admin);
        TestDataSeeder.SetCurrentUser(user);

        var nguyenLieuA = TestDataSeeder.CreateNguyenLieu(context, "Bot cacao", soLuongTon: 10m, giaNhapGanNhat: 5000m);
        var nguyenLieuB = TestDataSeeder.CreateNguyenLieu(context, "Sua tuoi", soLuongTon: 20m, giaNhapGanNhat: 7000m);

        var sut = new NguyenLieuDAL();
        var ketQua = sut.NhapKhoNhieuNguyenLieu(
            new[]
            {
                new NhapKhoChiTietDTO { NguyenLieuID = nguyenLieuA.ID, SoLuong = 3m, DonGiaNhap = 6000m },
                new NhapKhoChiTietDTO { NguyenLieuID = nguyenLieuB.ID, SoLuong = 4m, DonGiaNhap = 8000m }
            },
            ghiChu: "Nhap theo lo");

        ketQua.ThanhCong.Should().BeTrue();

        using var verifyContext = scope.CreateContext();
        verifyContext.PhieuNhapKho.Count().Should().Be(1);
        verifyContext.ChiTietPhieuNhap.Count().Should().Be(2);

        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieuA.ID).SoLuongTon.Should().Be(13m);
        verifyContext.NguyenLieu.First(x => x.ID == nguyenLieuB.ID).SoLuongTon.Should().Be(24m);
    }

    [Fact]
    public void XoaNguyenLieu_HasWarehouseHistory_FallsBackToInactiveStatus()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var user = TestDataSeeder.CreateUser(context, "warehouse_ref", "123", RoleEnum.Admin);
        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Sua xoa", soLuongTon: 10m, giaNhapGanNhat: 8000m, trangThai: 1);

        var phieuXuat = new dtaPhieuXuatKho
        {
            NgayXuat = DateTime.Now,
            LyDo = "Test xuat kho",
            NhanVienID = user.NhanVienId
        };

        context.PhieuXuatKho.Add(phieuXuat);
        context.ChiTietPhieuXuat.Add(new dtaChiTietPhieuXuat
        {
            PhieuXuat = phieuXuat,
            NguyenLieuID = nguyenLieu.ID,
            SoLuong = 2m
        });
        context.SaveChanges();

        var sut = new NguyenLieuDAL();
        var ketQua = sut.XoaNguyenLieu(nguyenLieu.ID);

        ketQua.ThanhCong.Should().BeTrue();
        ketQua.ThongBao.Should().Contain("ngừng dùng");

        using var verifyContext = scope.CreateContext();
        var nguyenLieuSauXoa = verifyContext.NguyenLieu.First(x => x.ID == nguyenLieu.ID);

        nguyenLieuSauXoa.TrangThai.Should().Be(0);
        nguyenLieuSauXoa.TrangThaiTextLegacy.Should().Be("Ngừng dùng");
    }

    [Fact]
    public void XoaNguyenLieu_NoReferences_DeletesIngredient()
    {
        using var scope = new SqliteTestScope();
        using var context = scope.CreateContext();

        var nguyenLieu = TestDataSeeder.CreateNguyenLieu(context, "Sua xoa cung", soLuongTon: 10m, giaNhapGanNhat: 8000m, trangThai: 1);

        var sut = new NguyenLieuDAL();
        var ketQua = sut.XoaNguyenLieu(nguyenLieu.ID);

        ketQua.ThanhCong.Should().BeTrue();
        ketQua.ThongBao.Should().Contain("Xóa nguyên liệu thành công");

        using var verifyContext = scope.CreateContext();
        verifyContext.NguyenLieu.Any(x => x.ID == nguyenLieu.ID).Should().BeFalse();
    }
}
