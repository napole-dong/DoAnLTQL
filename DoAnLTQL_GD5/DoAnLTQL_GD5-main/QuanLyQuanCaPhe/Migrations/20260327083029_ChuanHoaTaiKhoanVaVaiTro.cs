using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class ChuanHoaTaiKhoanVaVaiTro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VaiTro",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTro", x => x.ID);
                });

            migrationBuilder.DropIndex(
                name: "IX_User_NhanVienID",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_VaiTro_TenVaiTro",
                table: "VaiTro",
                column: "TenVaiTro",
                unique: true);

            migrationBuilder.Sql(@"INSERT INTO VaiTro (TenVaiTro, MoTa)
SELECT DISTINCT VaiTro, NULL
FROM [User]
WHERE VaiTro IS NOT NULL AND LTRIM(RTRIM(VaiTro)) <> ''; ");

            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE TenVaiTro = N'Nhân viên')
INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES (N'Nhân viên', N'Vai trò mặc định');");

            migrationBuilder.AddColumn<int>(
                name: "VaiTroID",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE U
SET U.VaiTroID = V.ID
FROM [User] U
INNER JOIN VaiTro V ON V.TenVaiTro = U.VaiTro;");

            migrationBuilder.Sql(@"UPDATE [User]
SET VaiTroID = (SELECT TOP 1 ID FROM VaiTro WHERE TenVaiTro = N'Nhân viên')
WHERE VaiTroID IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "VaiTroID",
                table: "User",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "VaiTro",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MatKhau",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "QuyenHan",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "TenDangNhap",
                table: "NhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_User_NhanVienID",
                table: "User",
                column: "NhanVienID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_VaiTroID",
                table: "User",
                column: "VaiTroID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_VaiTro_VaiTroID",
                table: "User",
                column: "VaiTroID",
                principalTable: "VaiTro",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_VaiTro_VaiTroID",
                table: "User");

            migrationBuilder.DropTable(
                name: "VaiTro");

            migrationBuilder.DropIndex(
                name: "IX_User_NhanVienID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_VaiTroID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "VaiTroID",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "VaiTro",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MatKhau",
                table: "NhanVien",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "QuyenHan",
                table: "NhanVien",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TenDangNhap",
                table: "NhanVien",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_NhanVienID",
                table: "User",
                column: "NhanVienID");
        }
    }
}
