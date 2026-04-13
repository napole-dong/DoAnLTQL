using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class P1_DbIntegrity_UniqueAndRangeConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE Ban
SET TenBan = CONCAT('Ban-', ID)
WHERE TenBan IS NULL OR LTRIM(RTRIM(TenBan)) = '';

;WITH BanDup AS
(
    SELECT ID,
           ROW_NUMBER() OVER (PARTITION BY UPPER(LTRIM(RTRIM(TenBan))) ORDER BY ID) AS RN
    FROM Ban
)
UPDATE b
SET TenBan = CONCAT(LEFT(LTRIM(RTRIM(b.TenBan)), 88), ' #', b.ID)
FROM Ban AS b
INNER JOIN BanDup AS d ON d.ID = b.ID
WHERE d.RN > 1;

UPDATE LoaiMon
SET TenLoai = CONCAT('LoaiMon-', ID)
WHERE TenLoai IS NULL OR LTRIM(RTRIM(TenLoai)) = '';

;WITH LoaiDup AS
(
    SELECT ID,
           ROW_NUMBER() OVER (PARTITION BY UPPER(LTRIM(RTRIM(TenLoai))) ORDER BY ID) AS RN
    FROM LoaiMon
    WHERE ISNULL(IsDeleted, 0) = 0
)
UPDATE lm
SET TenLoai = CONCAT(LEFT(LTRIM(RTRIM(lm.TenLoai)), 88), ' #', lm.ID)
FROM LoaiMon AS lm
INNER JOIN LoaiDup AS d ON d.ID = lm.ID
WHERE d.RN > 1;

UPDATE KhachHang
SET DienThoai = NULLIF(LTRIM(RTRIM(DienThoai)), '')
WHERE DienThoai IS NOT NULL;

;WITH KhachDup AS
(
    SELECT ID,
           ROW_NUMBER() OVER (PARTITION BY DienThoai ORDER BY ID) AS RN
    FROM KhachHang
    WHERE ISNULL(IsDeleted, 0) = 0
      AND DienThoai IS NOT NULL
)
UPDATE kh
SET DienThoai = NULL
FROM KhachHang AS kh
INNER JOIN KhachDup AS d ON d.ID = kh.ID
WHERE d.RN > 1;

UPDATE Mon
SET DonGia = CASE
    WHEN DonGia < 0 THEN 0
    WHEN DonGia > 1000000000 THEN 1000000000
    ELSE DonGia END;

UPDATE HoaDon
SET TongTien = CASE
    WHEN TongTien < 0 THEN 0
    WHEN TongTien > 1000000000000 THEN 1000000000000
    ELSE TongTien END;

UPDATE NguyenLieu
SET SoLuongTon = CASE WHEN SoLuongTon < 0 THEN 0 ELSE SoLuongTon END,
    GiaNhapGanNhat = CASE WHEN GiaNhapGanNhat < 0 THEN 0 ELSE GiaNhapGanNhat END;
");

            migrationBuilder.AddCheckConstraint(
                name: "CK_NguyenLieu_GiaNhapGanNhat_NonNegative",
                table: "NguyenLieu",
                sql: "[GiaNhapGanNhat] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_NguyenLieu_SoLuongTon_NonNegative",
                table: "NguyenLieu",
                sql: "[SoLuongTon] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mon_DonGia_Range",
                table: "Mon",
                sql: "[DonGia] >= 0 AND [DonGia] <= 1000000000");

            migrationBuilder.CreateIndex(
                name: "UX_LoaiMon_TenLoai_Active",
                table: "LoaiMon",
                column: "TenLoai",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UX_KhachHang_DienThoai_Active",
                table: "KhachHang",
                column: "DienThoai",
                unique: true,
                filter: "[IsDeleted] = 0 AND [DienThoai] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_HoaDon_TongTien_Range",
                table: "HoaDon",
                sql: "[TongTien] >= 0 AND [TongTien] <= 1000000000000");

            migrationBuilder.CreateIndex(
                name: "UX_Ban_TenBan",
                table: "Ban",
                column: "TenBan",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_NguyenLieu_GiaNhapGanNhat_NonNegative",
                table: "NguyenLieu");

            migrationBuilder.DropCheckConstraint(
                name: "CK_NguyenLieu_SoLuongTon_NonNegative",
                table: "NguyenLieu");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mon_DonGia_Range",
                table: "Mon");

            migrationBuilder.DropIndex(
                name: "UX_LoaiMon_TenLoai_Active",
                table: "LoaiMon");

            migrationBuilder.DropIndex(
                name: "UX_KhachHang_DienThoai_Active",
                table: "KhachHang");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HoaDon_TongTien_Range",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "UX_Ban_TenBan",
                table: "Ban");
        }
    }
}
