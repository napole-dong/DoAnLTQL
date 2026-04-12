using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteLoaiMonAndStopSellingDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LoaiMon",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "LoaiMon",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoaiMon",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_LoaiMon_IsDeleted",
                table: "LoaiMon",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LoaiMon_IsDeleted",
                table: "LoaiMon");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LoaiMon");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "LoaiMon");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoaiMon");
        }
    }
}
