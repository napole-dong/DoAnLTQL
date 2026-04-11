using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.Services.SoftDelete;

public sealed class SoftDeleteService : ISoftDeleteService
{
    private readonly ISoftDeleteRepository _softDeleteRepository;

    public SoftDeleteService(ISoftDeleteRepository? softDeleteRepository = null)
    {
        _softDeleteRepository = softDeleteRepository ?? new SoftDeleteRepository();
    }

    public bool SoftDelete<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete
    {
        return _softDeleteRepository.Remove(context, entity);
    }

    public void Restore<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete
    {
        _softDeleteRepository.Restore(context, entity);
    }

    public void HardDelete<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete
    {
        _softDeleteRepository.HardDelete(context, entity);
    }
}