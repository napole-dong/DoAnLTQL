using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace QuanLyQuanCaPhe.Data;

public class CaPheDbContext : DbContext
{
    public CaPheDbContext()
    {
    }

    public CaPheDbContext(DbContextOptions<CaPheDbContext> options) : base(options)
    {
    }

    public DbSet<dtaBan> Ban { get; set; } = null!;
    public DbSet<dtaLoaiMon> LoaiMon { get; set; } = null!;
    public DbSet<dtaMon> Mon { get; set; } = null!;
    public DbSet<dtaNhanVien> NhanVien { get; set; } = null!;
    public DbSet<dtaUser> User { get; set; } = null!;
    public DbSet<dtaVaiTro> VaiTro { get; set; } = null!;
    public DbSet<dtaPermission> Permission { get; set; } = null!;
    public DbSet<dtaKhachHang> KhachHang { get; set; } = null!;
    public DbSet<dtaHoadon> HoaDon { get; set; } = null!;
    public DbSet<dtHoaDon_ChiTiet> HoaDon_ChiTiet { get; set; } = null!;
    public DbSet<dtaNguyenLieu> NguyenLieu { get; set; } = null!;
    public DbSet<dtaPhieuNhapKho> PhieuNhapKho { get; set; } = null!;
    public DbSet<dtaCongThucMon> CongThucMon { get; set; } = null!;
    public DbSet<dtaPhieuXuatKho> PhieuXuatKho { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<dtaBan>(entity =>
        {
            entity.ToTable("Ban");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.TenBan)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.TrangThai)
                .IsRequired();
        });

        modelBuilder.Entity<dtaKhachHang>(entity =>
        {
            entity.ToTable("KhachHang");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.HoVaTen)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.DienThoai)
                .HasMaxLength(20)
                .IsRequired(false);

            entity.Property(x => x.DiaChi)
                .HasMaxLength(255)
                .IsRequired(false);
        });

        modelBuilder.Entity<dtaLoaiMon>(entity =>
        {
            entity.ToTable("LoaiMon");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.TenLoai)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.MoTa)
                .HasMaxLength(500)
                .IsRequired(false)
                .HasDefaultValue(string.Empty);
        });

        modelBuilder.Entity<dtaMon>(entity =>
        {
            entity.ToTable("Mon", table =>
            {
                table.HasCheckConstraint("CK_Mon_TrangThai", "[TrangThai] IN (0, 1, 2)");
            });

            entity.HasKey(x => x.ID);

            entity.Property(x => x.TenMon)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.DonGia)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.HinhAnh)
                .HasMaxLength(500)
                .IsRequired(false);

            entity.Property(x => x.MoTa)
                .HasMaxLength(1000)
                .IsRequired(false);

            entity.Property(x => x.TrangThai)
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(x => x.TrangThaiTextLegacy)
                .HasColumnName("TrangThaiText_Legacy")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("Đang kinh doanh");

            entity.HasIndex(x => x.LoaiMonID);

            entity.HasOne(x => x.LoaiMon)
                .WithMany(x => x.Mon)
                .HasForeignKey(x => x.LoaiMonID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<dtaNhanVien>(entity =>
        {
            entity.ToTable("NhanVien");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.HoVaTen)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.DienThoai)
                .HasMaxLength(20)
                .IsRequired(false);

            entity.Property(x => x.DiaChi)
                .HasMaxLength(255)
                .IsRequired(false);
        });

        modelBuilder.Entity<dtaHoadon>(entity =>
        {
            entity.ToTable("HoaDon");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.GhiChuHoaDon)
                .HasMaxLength(1000)
                .IsRequired(false);

            entity.Property(x => x.KhachHangID).IsRequired(false);

            entity.HasIndex(x => x.BanID);
            entity.HasIndex(x => x.KhachHangID);
            entity.HasIndex(x => x.NhanVienID);

            entity.HasOne(x => x.Ban)
                .WithMany(x => x.HoaDon)
                .HasForeignKey(x => x.BanID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.KhachHang)
                .WithMany(x => x.HoaDon)
                .HasForeignKey(x => x.KhachHangID)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.NhanVien)
                .WithMany(x => x.HoaDon)
                .HasForeignKey(x => x.NhanVienID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<dtHoaDon_ChiTiet>(entity =>
        {
            entity.ToTable("HoaDon_ChiTiet");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.DonGiaBan)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.GhiChu)
                .HasMaxLength(500)
                .IsRequired(false);

            entity.HasIndex(x => x.HoaDonID);
            entity.HasIndex(x => x.MonID);

            entity.HasOne(x => x.HoaDon)
                .WithMany(x => x.HoaDon_ChiTiet)
                .HasForeignKey(x => x.HoaDonID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Mon)
                .WithMany(x => x.HoaDon_ChiTiet)
                .HasForeignKey(x => x.MonID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<dtaNguyenLieu>(entity =>
        {
            entity.ToTable("NguyenLieu", table =>
            {
                table.HasCheckConstraint("CK_NguyenLieu_TrangThai", "[TrangThai] IN (0, 1, 2)");
            });

            entity.HasKey(x => x.ID);

            entity.Property(x => x.TenNguyenLieu)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.DonViTinh)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.SoLuongTon)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.MucCanhBao)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.GiaNhapGanNhat)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.TrangThai)
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(x => x.TrangThaiTextLegacy)
                .HasColumnName("TrangThaiText_Legacy")
                .IsRequired();
        });

        modelBuilder.Entity<dtaPhieuNhapKho>(entity =>
        {
            entity.ToTable("PhieuNhapKho");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.SoLuongNhap)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.GiaNhap)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.GhiChu)
                .HasMaxLength(500)
                .IsRequired(false);

            entity.HasIndex(x => x.NguyenLieuID);

            entity.HasOne(x => x.NguyenLieu)
                .WithMany(x => x.PhieuNhapKho)
                .HasForeignKey(x => x.NguyenLieuID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<dtaCongThucMon>(entity =>
        {
            entity.ToTable("CongThucMon", table =>
            {
                table.HasCheckConstraint("CK_CongThucMon_SoLuong", "[SoLuong] > 0");
            });

            entity.HasKey(x => new { x.MonID, x.NguyenLieuID });
            entity.Property(x => x.SoLuong)
                .HasColumnType("decimal(18,3)")
                .IsRequired();

            entity.HasOne(x => x.Mon)
                .WithMany(x => x.CongThucMon)
                .HasForeignKey(x => x.MonID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.NguyenLieu)
                .WithMany(x => x.CongThucMon)
                .HasForeignKey(x => x.NguyenLieuID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(x => x.NguyenLieuID);
        });

        modelBuilder.Entity<dtaPhieuXuatKho>(entity =>
        {
            entity.ToTable("PhieuXuatKho", table =>
            {
                table.HasCheckConstraint("CK_PhieuXuatKho_SoLuongXuat", "[SoLuongXuat] > 0");
            });

            entity.HasKey(x => x.ID);

            entity.Property(x => x.SoLuongXuat)
                .HasColumnType("decimal(18,3)")
                .IsRequired();

            entity.Property(x => x.NgayXuat).HasDefaultValueSql("SYSDATETIME()");
            entity.Property(x => x.LyDo)
                .HasMaxLength(500)
                .IsRequired();

            entity.HasOne(x => x.NguyenLieu)
                .WithMany(x => x.PhieuXuatKho)
                .HasForeignKey(x => x.NguyenLieuID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(x => new { x.NguyenLieuID, x.NgayXuat });
        });

        modelBuilder.Entity<dtaUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.ID).HasColumnName("Id");
            entity.Property(x => x.NhanVienID).HasColumnName("NhanVienId");
            entity.Property(x => x.VaiTroID).HasColumnName("RoleId");
            entity.Property(x => x.TenDangNhap)
                .HasColumnName("Username")
                .HasMaxLength(450)
                .IsRequired();

            entity.Property(x => x.MatKhau)
                .HasColumnName("Password")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.HoatDong).HasColumnName("IsActive");

            entity.HasIndex(x => x.TenDangNhap).IsUnique();
            entity.HasIndex(x => x.NhanVienID).IsUnique();
            entity.HasIndex(x => x.VaiTroID);

            entity.HasOne(x => x.NhanVien)
                .WithOne(x => x.User)
                .HasForeignKey<dtaUser>(x => x.NhanVienID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.VaiTro)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.VaiTroID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<dtaVaiTro>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(x => x.ID);

            entity.Property(x => x.ID).HasColumnName("Id");
            entity.Property(x => x.TenVaiTro)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.MoTa)
                .HasColumnName("Description")
                .HasMaxLength(500)
                .IsRequired(false);

            entity.HasIndex(x => x.TenVaiTro).IsUnique();
        });

        modelBuilder.Entity<dtaPermission>(entity =>
        {
            entity.ToTable("Permissions");
            entity.HasKey(x => new { x.VaiTroID, x.Feature });

            entity.Property(x => x.VaiTroID).HasColumnName("RoleId");
            entity.Property(x => x.Feature)
                .HasColumnName("Feature")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.CanView).HasColumnName("CanView");
            entity.Property(x => x.CanCreate).HasColumnName("CanCreate");
            entity.Property(x => x.CanUpdate).HasColumnName("CanUpdate");
            entity.Property(x => x.CanDelete).HasColumnName("CanDelete");

            entity.HasOne(x => x.VaiTro)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.VaiTroID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var connectionString = ConfigurationManager.ConnectionStrings["CaPheConnection"]?.ConnectionString;
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Không tìm thấy connection string 'CaPheConnection' trong App.config.");
        }

        optionsBuilder.UseSqlServer(connectionString);
    }
}
