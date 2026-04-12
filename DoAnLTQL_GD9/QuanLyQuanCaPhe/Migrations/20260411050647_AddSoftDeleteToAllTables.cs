using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "NhanVien",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "NhanVien",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "NhanVien",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Mon",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Mon",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Mon",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "KhachHang",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_IsDeleted",
                table: "NhanVien",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Mon_IsDeleted",
                table: "Mon",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NhanVien_IsDeleted",
                table: "NhanVien");

            migrationBuilder.DropIndex(
                name: "IX_Mon_IsDeleted",
                table: "Mon");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Mon");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Mon");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Mon");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "KhachHang");
        }
    }
}
