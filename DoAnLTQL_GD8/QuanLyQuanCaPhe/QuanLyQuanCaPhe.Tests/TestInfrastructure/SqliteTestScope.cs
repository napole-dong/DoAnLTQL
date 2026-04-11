using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.Tests.TestInfrastructure;

public sealed class SqliteTestScope : IDisposable
{
    private readonly string _databaseName;
    private readonly IDisposable _ambientScope;
    private readonly string _connectionString;

    public SqliteTestScope()
    {
        _databaseName = $"QLQCP_Tests_{Guid.NewGuid():N}";
        _connectionString = BuildConnectionString(_databaseName);

        Options = new DbContextOptionsBuilder<CaPheDbContext>()
            .UseSqlServer(_connectionString, sql => sql.EnableRetryOnFailure())
            .EnableSensitiveDataLogging()
            .Options;

        _ambientScope = CaPheDbContext.PushAmbientOptions(Options);

        using var context = new CaPheDbContext(Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public DbContextOptions<CaPheDbContext> Options { get; }

    public CaPheDbContext CreateContext()
    {
        return new CaPheDbContext(Options);
    }

    public void Dispose()
    {
        NguoiDungHienTaiService.XoaNguoiDungDangNhap();

        try
        {
            using var context = new CaPheDbContext(Options);
            context.Database.EnsureDeleted();
        }
        catch
        {
            // Best-effort cleanup for ephemeral test database.
        }

        _ambientScope.Dispose();
    }

    private static string BuildConnectionString(string databaseName)
    {
        var server = Environment.GetEnvironmentVariable("QLQCP_TEST_SQLSERVER");
        if (string.IsNullOrWhiteSpace(server))
        {
            server = @".\SQLEXPRESS";
        }

        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
        {
            DataSource = server,
            InitialCatalog = databaseName,
            IntegratedSecurity = true,
            Encrypt = false,
            TrustServerCertificate = true,
            MultipleActiveResultSets = true,
            ConnectTimeout = 30
        };

        return builder.ConnectionString;
    }
}
