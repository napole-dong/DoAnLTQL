using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class P3_CustomerSoftDeleteInvoiceSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "KhachHang",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KhachHang",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "HoaDon",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "Khách lẻ");

            migrationBuilder.Sql(@"
                UPDATE hd
                SET hd.CustomerName = COALESCE(NULLIF(kh.HoVaTen, N''), N'Khách lẻ')
                FROM HoaDon hd
                LEFT JOIN KhachHang kh ON kh.ID = hd.KhachHangID;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_IsDeleted",
                table: "KhachHang",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KhachHang_IsDeleted",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "HoaDon");
        }
    }
}
