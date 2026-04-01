using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class BoSungTrangThaiMonVaMoTaLoaiMon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "Mon",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Đang kinh doanh");

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "LoaiMon",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "Mon");

            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "LoaiMon");
        }
    }
}
