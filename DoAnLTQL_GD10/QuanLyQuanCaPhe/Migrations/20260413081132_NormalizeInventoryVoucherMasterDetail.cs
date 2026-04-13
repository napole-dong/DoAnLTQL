using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeInventoryVoucherMasterDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapKho_NguyenLieu_NguyenLieuID",
                table: "PhieuNhapKho");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuXuatKho_NguyenLieu_NguyenLieuID",
                table: "PhieuXuatKho");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PhieuXuatKho_SoLuongXuat",
                table: "PhieuXuatKho");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNhapKho_NguyenLieuID",
                table: "PhieuNhapKho");

            migrationBuilder.RenameColumn(
                name: "NguyenLieuID",
                table: "PhieuXuatKho",
                newName: "NhanVienID");

            migrationBuilder.RenameIndex(
                name: "IX_PhieuXuatKho_NguyenLieuID_NgayXuat",
                table: "PhieuXuatKho",
                newName: "IX_PhieuXuatKho_NhanVienID_NgayXuat");

            migrationBuilder.RenameColumn(
                name: "NguyenLieuID",
                table: "PhieuNhapKho",
                newName: "NhanVienID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayNhap",
                table: "PhieuNhapKho",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuNhap",
                columns: table => new
                {
                    PhieuNhapID = table.Column<int>(type: "int", nullable: false),
                    NguyenLieuID = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    DonGiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPhieuNhap", x => new { x.PhieuNhapID, x.NguyenLieuID });
                    table.CheckConstraint("CK_ChiTietPhieuNhap_DonGiaNhap", "[DonGiaNhap] >= 0");
                    table.CheckConstraint("CK_ChiTietPhieuNhap_SoLuong", "[SoLuong] > 0");
                    table.CheckConstraint("CK_ChiTietPhieuNhap_ThanhTien", "[ThanhTien] >= 0");
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuNhap_NguyenLieu_NguyenLieuID",
                        column: x => x.NguyenLieuID,
                        principalTable: "NguyenLieu",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuNhap_PhieuNhapKho_PhieuNhapID",
                        column: x => x.PhieuNhapID,
                        principalTable: "PhieuNhapKho",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuXuat",
                columns: table => new
                {
                    PhieuXuatID = table.Column<int>(type: "int", nullable: false),
                    NguyenLieuID = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPhieuXuat", x => new { x.PhieuXuatID, x.NguyenLieuID });
                    table.CheckConstraint("CK_ChiTietPhieuXuat_SoLuong", "[SoLuong] > 0");
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuXuat_NguyenLieu_NguyenLieuID",
                        column: x => x.NguyenLieuID,
                        principalTable: "NguyenLieu",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuXuat_PhieuXuatKho_PhieuXuatID",
                        column: x => x.PhieuXuatID,
                        principalTable: "PhieuXuatKho",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(
                @"
INSERT INTO [ChiTietPhieuNhap] ([PhieuNhapID], [NguyenLieuID], [SoLuong], [DonGiaNhap], [ThanhTien])
SELECT
    [ID],
    [NhanVienID],
    CAST([SoLuongNhap] AS decimal(18,3)),
    [GiaNhap],
    ROUND([SoLuongNhap] * [GiaNhap], 2)
FROM [PhieuNhapKho];

INSERT INTO [ChiTietPhieuXuat] ([PhieuXuatID], [NguyenLieuID], [SoLuong])
SELECT
    [ID],
    [NhanVienID],
    CAST([SoLuongXuat] AS decimal(18,3))
FROM [PhieuXuatKho];

DECLARE @NhanVienMacDinh INT = (SELECT TOP (1) [ID] FROM [NhanVien] ORDER BY [ID]);
IF @NhanVienMacDinh IS NULL
BEGIN
    INSERT INTO [NhanVien] ([HoVaTen], [DienThoai], [DiaChi], [IsDeleted], [DeletedAt], [DeletedBy])
    VALUES (N'Hệ thống kho', NULL, NULL, 0, NULL, NULL);
    SET @NhanVienMacDinh = CAST(SCOPE_IDENTITY() AS int);
END;

UPDATE [PhieuNhapKho]
SET [NhanVienID] = @NhanVienMacDinh;

UPDATE [PhieuXuatKho]
SET [NhanVienID] = @NhanVienMacDinh;
");

            migrationBuilder.DropColumn(
                name: "SoLuongXuat",
                table: "PhieuXuatKho");

            migrationBuilder.DropColumn(
                name: "GiaNhap",
                table: "PhieuNhapKho");

            migrationBuilder.DropColumn(
                name: "SoLuongNhap",
                table: "PhieuNhapKho");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapKho_NhanVienID_NgayNhap",
                table: "PhieuNhapKho",
                columns: new[] { "NhanVienID", "NgayNhap" });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhap_NguyenLieuID",
                table: "ChiTietPhieuNhap",
                column: "NguyenLieuID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuXuat_NguyenLieuID",
                table: "ChiTietPhieuXuat",
                column: "NguyenLieuID");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapKho_NhanVien_NhanVienID",
                table: "PhieuNhapKho",
                column: "NhanVienID",
                principalTable: "NhanVien",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuXuatKho_NhanVien_NhanVienID",
                table: "PhieuXuatKho",
                column: "NhanVienID",
                principalTable: "NhanVien",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapKho_NhanVien_NhanVienID",
                table: "PhieuNhapKho");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuXuatKho_NhanVien_NhanVienID",
                table: "PhieuXuatKho");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNhapKho_NhanVienID_NgayNhap",
                table: "PhieuNhapKho");

            migrationBuilder.RenameColumn(
                name: "NhanVienID",
                table: "PhieuXuatKho",
                newName: "NguyenLieuID");

            migrationBuilder.RenameIndex(
                name: "IX_PhieuXuatKho_NhanVienID_NgayXuat",
                table: "PhieuXuatKho",
                newName: "IX_PhieuXuatKho_NguyenLieuID_NgayXuat");

            migrationBuilder.RenameColumn(
                name: "NhanVienID",
                table: "PhieuNhapKho",
                newName: "NguyenLieuID");

            migrationBuilder.AddColumn<decimal>(
                name: "SoLuongXuat",
                table: "PhieuXuatKho",
                type: "decimal(18,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayNhap",
                table: "PhieuNhapKho",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSDATETIME()");

            migrationBuilder.AddColumn<decimal>(
                name: "GiaNhap",
                table: "PhieuNhapKho",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SoLuongNhap",
                table: "PhieuNhapKho",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(
                @"
;WITH [NhapKhoFirst] AS
(
    SELECT
        [PhieuNhapID],
        [NguyenLieuID],
        [SoLuong],
        [DonGiaNhap],
        ROW_NUMBER() OVER (PARTITION BY [PhieuNhapID] ORDER BY [NguyenLieuID]) AS [rn]
    FROM [ChiTietPhieuNhap]
),
[XuatKhoFirst] AS
(
    SELECT
        [PhieuXuatID],
        [NguyenLieuID],
        [SoLuong],
        ROW_NUMBER() OVER (PARTITION BY [PhieuXuatID] ORDER BY [NguyenLieuID]) AS [rn]
    FROM [ChiTietPhieuXuat]
)
UPDATE [p]
SET
    [p].[NguyenLieuID] = [n].[NguyenLieuID],
    [p].[SoLuongNhap] = CAST([n].[SoLuong] AS decimal(18,2)),
    [p].[GiaNhap] = [n].[DonGiaNhap]
FROM [PhieuNhapKho] AS [p]
INNER JOIN [NhapKhoFirst] AS [n]
    ON [p].[ID] = [n].[PhieuNhapID]
    AND [n].[rn] = 1;

UPDATE [p]
SET
    [p].[NguyenLieuID] = [x].[NguyenLieuID],
    [p].[SoLuongXuat] = CAST([x].[SoLuong] AS decimal(18,3))
FROM [PhieuXuatKho] AS [p]
INNER JOIN [XuatKhoFirst] AS [x]
    ON [p].[ID] = [x].[PhieuXuatID]
    AND [x].[rn] = 1;
");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PhieuXuatKho_SoLuongXuat",
                table: "PhieuXuatKho",
                sql: "[SoLuongXuat] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapKho_NguyenLieuID",
                table: "PhieuNhapKho",
                column: "NguyenLieuID");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapKho_NguyenLieu_NguyenLieuID",
                table: "PhieuNhapKho",
                column: "NguyenLieuID",
                principalTable: "NguyenLieu",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuXuatKho_NguyenLieu_NguyenLieuID",
                table: "PhieuXuatKho",
                column: "NguyenLieuID",
                principalTable: "NguyenLieu",
                principalColumn: "ID");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuNhap");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuXuat");
        }
    }
}
