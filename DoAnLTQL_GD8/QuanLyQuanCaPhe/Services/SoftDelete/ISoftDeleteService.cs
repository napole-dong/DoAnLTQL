using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.Services.SoftDelete;

public interface ISoftDeleteService
{
    bool SoftDelete<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete;

    void Restore<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete;

    void HardDelete<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete;
}