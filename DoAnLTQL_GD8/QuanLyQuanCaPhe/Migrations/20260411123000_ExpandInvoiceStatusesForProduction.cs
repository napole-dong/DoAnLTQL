using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class ExpandInvoiceStatusesForProduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_HoaDon_TrangThai",
                table: "HoaDon");

            // Legacy status mapping: old value 2 means cancelled.
            migrationBuilder.Sql("UPDATE [HoaDon] SET [TrangThai] = 3 WHERE [TrangThai] = 2;");

            migrationBuilder.AddCheckConstraint(
                name: "CK_HoaDon_TrangThai",
                table: "HoaDon",
                sql: "[TrangThai] IN (0, 1, 2, 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_HoaDon_TrangThai",
                table: "HoaDon");

            // Down-mapping for old schema without Closed status.
            migrationBuilder.Sql("UPDATE [HoaDon] SET [TrangThai] = 1 WHERE [TrangThai] = 2;");
            migrationBuilder.Sql("UPDATE [HoaDon] SET [TrangThai] = 2 WHERE [TrangThai] = 3;");

            migrationBuilder.AddCheckConstraint(
                name: "CK_HoaDon_TrangThai",
                table: "HoaDon",
                sql: "[TrangThai] IN (0, 1, 2)");
        }
    }
}
