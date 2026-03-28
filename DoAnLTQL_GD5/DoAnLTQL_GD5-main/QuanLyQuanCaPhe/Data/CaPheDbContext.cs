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
    public DbSet<dtaKhachHang> KhachHang { get; set; } = null!;
    public DbSet<dtaHoadon> HoaDon { get; set; } = null!;
    public DbSet<dtHoaDon_ChiTiet> HoaDon_ChiTiet { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<dtaUser>(entity =>
        {
            entity.ToTable("User");
            entity.HasIndex(x => x.TenDangNhap).IsUnique();
            entity.HasIndex(x => x.NhanVienID).IsUnique();

            entity.HasOne(x => x.NhanVien)
                .WithOne(x => x.User)
                .HasForeignKey<dtaUser>(x => x.NhanVienID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.VaiTro)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.VaiTroID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<dtaVaiTro>(entity =>
        {
            entity.ToTable("VaiTro");
            entity.HasIndex(x => x.TenVaiTro).IsUnique();
        });
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
