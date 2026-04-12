using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace QuanLyQuanCaPhe.Data;

public class CaPheDbContext : DbContext
{
    private static readonly AsyncLocal<DbContextOptions<CaPheDbContext>?> AmbientOptions = new();
    private readonly Func<string?> _currentUserProvider;

    public CaPheDbContext()
        : this(AmbientOptions.Value ?? new DbContextOptions<CaPheDbContext>(), currentUserProvider: null)
    {
    }

    public CaPheDbContext(Func<string?>? currentUserProvider)
        : this(AmbientOptions.Value ?? new DbContextOptions<CaPheDbContext>(), currentUserProvider)
    {
    }

    public static IDisposable PushAmbientOptions(DbContextOptions<CaPheDbContext> options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var previousOptions = AmbientOptions.Value;
        AmbientOptions.Value = options;
        return new AmbientOptionsScope(previousOptions);
    }

    private sealed class AmbientOptionsScope : IDisposable
    {
        private readonly DbContextOptions<CaPheDbContext>? _previousOptions;
        private bool _disposed;

        public AmbientOptionsScope(DbContextOptions<CaPheDbContext>? previousOptions)
        {
            _previousOptions = previousOptions;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            AmbientOptions.Value = _previousOptions;
            _disposed = true;
        }
    }

    public CaPheDbContext(DbContextOptions<CaPheDbContext> options, Func<string?>? currentUserProvider)
        : base(options)
    {
        _currentUserProvider = currentUserProvider ?? DefaultCurrentUserProvider;
    }

    public CaPheDbContext(DbContextOptions<CaPheDbContext> options)
        : this(options, currentUserProvider: null)
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
    public DbSet<dtaAuditLog> AuditLog { get; set; } = null!;
    public DbSet<dtaNguyenLieu> NguyenLieu { get; set; } = null!;
    public DbSet<dtaPhieuNhapKho> PhieuNhapKho { get; set; } = null!;
    public DbSet<dtaCongThucMon> CongThucMon { get; set; } = null!;
    public DbSet<dtaPhieuXuatKho> PhieuXuatKho { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<dtaBan>(entity =>
        {
            entity.ToTable("Ban", table =>
            {
                table.HasCheckConstraint("CK_Ban_TrangThai", "[TrangThai] IN (0, 1, 2)");
            });
            entity.HasKey(x => x.ID);

            entity.Property(x => x.TenBan)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.TrangThai)
                .IsRequired();

            entity.HasIndex(x => x.TrangThai)
                .HasDatabaseName("IX_Ban_TrangThai");
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
            entity.ToTable("HoaDon", table =>
            {
                table.HasCheckConstraint("CK_HoaDon_TrangThai", "[TrangThai] IN (0, 1, 2, 3)");
            });
            entity.HasKey(x => x.ID);

            entity.Property(x => x.CustomerName)
                .HasMaxLength(150)
                .IsRequired()
                .HasDefaultValue("Khách lẻ");

            entity.Property(x => x.TongTien)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasDefaultValue(0m);

            entity.Property(x => x.RowVersion)
                .IsRowVersion();

            entity.Property(x => x.GhiChuHoaDon)
                .HasMaxLength(1000)
                .IsRequired(false);

            entity.Property(x => x.KhachHangID).IsRequired(false);

            entity.Property(x => x.TrangThai)
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasIndex(x => x.BanID);
            entity.HasIndex(x => x.BanID)
                .HasDatabaseName("UX_HoaDon_Ban_Mo")
                .HasFilter("[TrangThai] = 0")
                .IsUnique();
            entity.HasIndex(x => x.KhachHangID);
            entity.HasIndex(x => x.NhanVienID);
            entity.HasIndex(x => new { x.NgayLap, x.TrangThai })
                .HasDatabaseName("IX_HoaDon_NgayLap_TrangThai");
            entity.HasIndex(x => new { x.TrangThai, x.NhanVienID, x.NgayLap })
                .HasDatabaseName("IX_HoaDon_TrangThai_NhanVien_NgayLap");

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
            entity.ToTable("HoaDon_ChiTiet", table =>
            {
                table.HasCheckConstraint("CK_HoaDonChiTiet_SoLuongBan", "[SoLuongBan] > 0");
            });
            entity.HasKey(x => x.ID);

            entity.Property(x => x.DonGiaBan)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.ThanhTien)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.GhiChu)
                .HasMaxLength(500)
                .IsRequired(false);

            entity.HasIndex(x => x.HoaDonID);
            entity.HasIndex(x => x.MonID);
            entity.HasIndex(x => new { x.HoaDonID, x.MonID })
                .HasDatabaseName("IX_HoaDonChiTiet_HoaDonID_MonID");

            entity.HasOne(x => x.HoaDon)
                .WithMany(x => x.HoaDon_ChiTiet)
                .HasForeignKey(x => x.HoaDonID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Mon)
                .WithMany(x => x.HoaDon_ChiTiet)
                .HasForeignKey(x => x.MonID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<dtaAuditLog>(entity =>
        {
            entity.ToTable("AuditLog");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Action)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.EntityName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.EntityId)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.OldValue)
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(x => x.NewValue)
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(x => x.PerformedBy)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("SYSDATETIME()")
                .IsRequired();

            entity.HasIndex(x => x.CreatedAt);
            entity.HasIndex(x => new { x.EntityName, x.EntityId, x.CreatedAt });
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
                .OnDelete(DeleteBehavior.Cascade);

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

        ApplySoftDeleteColumnConfiguration(modelBuilder);
        ApplySoftDeleteQueryFilters(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplySoftDeleteRules();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplySoftDeleteRules();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var connectionString = ConnectionStringResolver.Resolve();
        optionsBuilder.UseSqlServer(
            connectionString,
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null));
    }

    private static void ApplySoftDeleteColumnConfiguration(ModelBuilder modelBuilder)
    {
        foreach (var clrType in GetSoftDeleteEntityTypes(modelBuilder))
        {
            var method = typeof(CaPheDbContext)
                .GetMethod(nameof(ConfigureSoftDeleteEntity), BindingFlags.NonPublic | BindingFlags.Static)?
                .MakeGenericMethod(clrType);

            method?.Invoke(null, new object[] { modelBuilder });
        }
    }

    private static void ConfigureSoftDeleteEntity<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ISoftDelete
    {
        var entity = modelBuilder.Entity<TEntity>();
        entity.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        entity.Property(x => x.DeletedAt)
            .IsRequired(false);

        entity.Property(x => x.DeletedBy)
            .HasMaxLength(150)
            .IsRequired(false);

        entity.HasIndex(x => x.IsDeleted);
    }

    private static void ApplySoftDeleteQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var clrType in GetSoftDeleteEntityTypes(modelBuilder))
        {
            var method = typeof(CaPheDbContext)
                .GetMethod(nameof(ApplySoftDeleteQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)?
                .MakeGenericMethod(clrType);

            method?.Invoke(null, new object[] { modelBuilder });
        }
    }

    private static void ApplySoftDeleteQueryFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ISoftDelete
    {
        Expression<Func<TEntity, bool>> filter = entity => !entity.IsDeleted;
        modelBuilder.Entity<TEntity>().HasQueryFilter(filter);
    }

    private static IEnumerable<Type> GetSoftDeleteEntityTypes(ModelBuilder modelBuilder)
    {
        return modelBuilder.Model
            .GetEntityTypes()
            .Where(x => typeof(ISoftDelete).IsAssignableFrom(x.ClrType))
            .Select(x => x.ClrType);
    }

    private void ApplySoftDeleteRules()
    {
        var deletedEntries = ChangeTracker
            .Entries<ISoftDelete>()
            .Where(x => x.State == EntityState.Deleted)
            .ToList();

        if (deletedEntries.Count == 0)
        {
            return;
        }

        var deletedAt = DateTime.Now;
        var deletedBy = GetCurrentUser();

        foreach (var entry in deletedEntries)
        {
            if (entry.Entity.IsDeleted)
            {
                continue;
            }

            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt = deletedAt;
            entry.Entity.DeletedBy = deletedBy;
        }
    }

    private string GetCurrentUser()
    {
        var currentUser = _currentUserProvider?.Invoke();
        return string.IsNullOrWhiteSpace(currentUser) ? "SYSTEM" : currentUser.Trim();
    }

    private static string? DefaultCurrentUserProvider()
    {
        return NguoiDungHienTaiService.LaySession()?.Username;
    }
}
