using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace QuanLyQuanCaPhe.Data
{
    public class CaPheDbContext : DbContext
    {
        public DbSet<Ban> Ban { get; set; }
        public DbSet<LoaiMon> LoaiMon { get; set; }
        public DbSet<Mon> Mon { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<HoaDon_ChiTiet> HoaDon_ChiTiet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Thay "CaPheConnection" bằng tên thẻ connectionString trong file App.config của bạn
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["CaPheConnection"].ConnectionString);
        }
    }
}