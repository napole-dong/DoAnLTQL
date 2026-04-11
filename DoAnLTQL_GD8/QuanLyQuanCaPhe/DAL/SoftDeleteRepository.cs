using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.DAL;

public sealed class SoftDeleteRepository : ISoftDeleteRepository
{
    public bool Remove<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete
    {
        if (entity.IsDeleted)
        {
            return false;
        }

        context.Remove(entity);
        context.SaveChanges();
        return true;
    }

    public void Restore<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete
    {
        if (!entity.IsDeleted)
        {
            return;
        }

        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = null;
        context.Entry(entity).State = EntityState.Modified;
        context.SaveChanges();
    }

    public void HardDelete<TEntity>(CaPheDbContext context, TEntity entity)
        where TEntity : class, ISoftDelete
    {
        if (!entity.IsDeleted)
        {
            throw new InvalidOperationException("Hard delete chi duoc phep voi ban ghi da soft delete truoc do.");
        }

        context.Entry(entity).State = EntityState.Deleted;
        context.SaveChanges();
    }
}