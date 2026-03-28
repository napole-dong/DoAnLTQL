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
    public DbSet<dtaKhachHang> KhachHang { get; set; } = null!;
    public DbSet<dtaHoadon> HoaDon { get; set; } = null!;
    public DbSet<dtHoaDon_ChiTiet> HoaDon_ChiTiet { get; set; } = null!;

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
