using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class BaselineNormalizedSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ban",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ban", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoVaTen = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LoaiMon",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiMon", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NguyenLieu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguyenLieu = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoLuongTon = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MucCanhBao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaNhapGanNhat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThaiText_Legacy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguyenLieu", x => x.ID);
                    table.CheckConstraint("CK_NguyenLieu_TrangThai", "[TrangThai] IN (0, 1, 2)");
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoVaTen = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mon",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiMonID = table.Column<int>(type: "int", nullable: false),
                    TenMon = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThaiText_Legacy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mon", x => x.ID);
                    table.CheckConstraint("CK_Mon_TrangThai", "[TrangThai] IN (0, 1, 2)");
                    table.ForeignKey(
                        name: "FK_Mon_LoaiMon_LoaiMonID",
                        column: x => x.LoaiMonID,
                        principalTable: "LoaiMon",
                        principalColumn: "ID");
                });

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
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNhapKho", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PhieuNhapKho_NguyenLieu_NguyenLieuID",
                        column: x => x.NguyenLieuID,
                        principalTable: "NguyenLieu",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PhieuXuatKho",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguyenLieuID = table.Column<int>(type: "int", nullable: false),
                    SoLuongXuat = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    NgayXuat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuXuatKho", x => x.ID);
                    table.CheckConstraint("CK_PhieuXuatKho_SoLuongXuat", "[SoLuongXuat] > 0");
                    table.ForeignKey(
                        name: "FK_PhieuXuatKho_NguyenLieu_NguyenLieuID",
                        column: x => x.NguyenLieuID,
                        principalTable: "NguyenLieu",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NhanVienID = table.Column<int>(type: "int", nullable: false),
                    KhachHangID = table.Column<int>(type: "int", nullable: true),
                    BanID = table.Column<int>(type: "int", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    GhiChuHoaDon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HoaDon_Ban_BanID",
                        column: x => x.BanID,
                        principalTable: "Ban",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HoaDon_KhachHang_KhachHangID",
                        column: x => x.KhachHangID,
                        principalTable: "KhachHang",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HoaDon_NhanVien_NhanVienID",
                        column: x => x.NhanVienID,
                        principalTable: "NhanVien",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Feature = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CanCreate = table.Column<bool>(type: "bit", nullable: false),
                    CanUpdate = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.RoleId, x.Feature });
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NhanVienId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_NhanVien_NhanVienId",
                        column: x => x.NhanVienId,
                        principalTable: "NhanVien",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CongThucMon",
                columns: table => new
                {
                    MonID = table.Column<int>(type: "int", nullable: false),
                    NguyenLieuID = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongThucMon", x => new { x.MonID, x.NguyenLieuID });
                    table.CheckConstraint("CK_CongThucMon_SoLuong", "[SoLuong] > 0");
                    table.ForeignKey(
                        name: "FK_CongThucMon_Mon_MonID",
                        column: x => x.MonID,
                        principalTable: "Mon",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CongThucMon_NguyenLieu_NguyenLieuID",
                        column: x => x.NguyenLieuID,
                        principalTable: "NguyenLieu",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon_ChiTiet",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoaDonID = table.Column<int>(type: "int", nullable: false),
                    MonID = table.Column<int>(type: "int", nullable: false),
                    SoLuongBan = table.Column<short>(type: "smallint", nullable: false),
                    DonGiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon_ChiTiet", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_HoaDon_HoaDonID",
                        column: x => x.HoaDonID,
                        principalTable: "HoaDon",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_Mon_MonID",
                        column: x => x.MonID,
                        principalTable: "Mon",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CongThucMon_NguyenLieuID",
                table: "CongThucMon",
                column: "NguyenLieuID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_BanID",
                table: "HoaDon",
                column: "BanID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_KhachHangID",
                table: "HoaDon",
                column: "KhachHangID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_NhanVienID",
                table: "HoaDon",
                column: "NhanVienID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ChiTiet_HoaDonID",
                table: "HoaDon_ChiTiet",
                column: "HoaDonID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ChiTiet_MonID",
                table: "HoaDon_ChiTiet",
                column: "MonID");

            migrationBuilder.CreateIndex(
                name: "IX_Mon_LoaiMonID",
                table: "Mon",
                column: "LoaiMonID");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapKho_NguyenLieuID",
                table: "PhieuNhapKho",
                column: "NguyenLieuID");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuatKho_NguyenLieuID_NgayXuat",
                table: "PhieuXuatKho",
                columns: new[] { "NguyenLieuID", "NgayXuat" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NhanVienId",
                table: "Users",
                column: "NhanVienId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CongThucMon");

            migrationBuilder.DropTable(
                name: "HoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PhieuNhapKho");

            migrationBuilder.DropTable(
                name: "PhieuXuatKho");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "Mon");

            migrationBuilder.DropTable(
                name: "NguyenLieu");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Ban");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "LoaiMon");
        }
    }
}
