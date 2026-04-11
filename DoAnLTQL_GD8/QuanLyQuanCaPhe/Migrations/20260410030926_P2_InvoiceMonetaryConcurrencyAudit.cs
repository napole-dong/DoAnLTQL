using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class P2_InvoiceMonetaryConcurrencyAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ThanhTien",
                table: "HoaDon_ChiTiet",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "HoaDon",
                type: "rowversion",
                rowVersion: true,
                nullable: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTien",
                table: "HoaDon",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.Sql(@"
UPDATE HoaDon_ChiTiet
SET SoLuongBan = 1
WHERE SoLuongBan <= 0;
");

            migrationBuilder.Sql(@"
UPDATE ct
SET ct.ThanhTien = ROUND(CAST(ct.SoLuongBan AS decimal(18,2)) * ct.DonGiaBan, 2)
FROM HoaDon_ChiTiet AS ct;
");

            migrationBuilder.Sql(@"
UPDATE hd
SET hd.TongTien = ISNULL(t.TongTien, 0)
FROM HoaDon AS hd
OUTER APPLY
(
    SELECT SUM(ct.ThanhTien) AS TongTien
    FROM HoaDon_ChiTiet AS ct
    WHERE ct.HoaDonID = hd.ID
) AS t;
");

            migrationBuilder.AddCheckConstraint(
                name: "CK_HoaDonChiTiet_SoLuongBan",
                table: "HoaDon_ChiTiet",
                sql: "[SoLuongBan] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CreatedAt",
                table: "AuditLog",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_EntityName_EntityId_CreatedAt",
                table: "AuditLog",
                columns: new[] { "EntityName", "EntityId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HoaDonChiTiet_SoLuongBan",
                table: "HoaDon_ChiTiet");

            migrationBuilder.DropColumn(
                name: "ThanhTien",
                table: "HoaDon_ChiTiet");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "HoaDon");
        }
    }
}
