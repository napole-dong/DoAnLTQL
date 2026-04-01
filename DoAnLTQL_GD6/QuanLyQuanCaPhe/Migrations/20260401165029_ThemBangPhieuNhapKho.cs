using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class ThemBangPhieuNhapKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhieuNhapKho",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguyenLieuID = table.Column<int>(type: "int", nullable: false),
                    SoLuongNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNhapKho", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PhieuNhapKho_NguyenLieu_NguyenLieuID",
                        column: x => x.NguyenLieuID,
                        principalTable: "NguyenLieu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapKho_NguyenLieuID",
                table: "PhieuNhapKho",
                column: "NguyenLieuID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhieuNhapKho");
        }
    }
}
