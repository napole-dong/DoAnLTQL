using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class P1_DbHardening_StatusChecks_ReportingIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "HoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_HoaDonID_MonID",
                table: "HoaDon_ChiTiet",
                columns: new[] { "HoaDonID", "MonID" });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_NgayLap_TrangThai",
                table: "HoaDon",
                columns: new[] { "NgayLap", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_TrangThai_NhanVien_NgayLap",
                table: "HoaDon",
                columns: new[] { "TrangThai", "NhanVienID", "NgayLap" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_HoaDon_TrangThai",
                table: "HoaDon",
                sql: "[TrangThai] IN (0, 1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_Ban_TrangThai",
                table: "Ban",
                column: "TrangThai");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Ban_TrangThai",
                table: "Ban",
                sql: "[TrangThai] IN (0, 1, 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HoaDonChiTiet_HoaDonID_MonID",
                table: "HoaDon_ChiTiet");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_NgayLap_TrangThai",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_TrangThai_NhanVien_NgayLap",
                table: "HoaDon");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HoaDon_TrangThai",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_Ban_TrangThai",
                table: "Ban");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Ban_TrangThai",
                table: "Ban");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "HoaDon",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
