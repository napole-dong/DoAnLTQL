using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class EnforceSingleOpenInvoicePerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HoaDon_BanID",
                table: "HoaDon");

            migrationBuilder.CreateIndex(
                name: "UX_HoaDon_Ban_Mo",
                table: "HoaDon",
                column: "BanID",
                unique: true,
                filter: "[TrangThai] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_HoaDon_Ban_Mo",
                table: "HoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_BanID",
                table: "HoaDon",
                column: "BanID");
        }
    }
}
