using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.DAL;

public interface ISoftDeleteRepository
{
    bool Remove<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete;

    void Restore<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete;

    void HardDelete<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete;
}