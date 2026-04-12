using System.Data;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.DAL;

internal static class ExecutionStrategyTransactionRunner
{
    public static async Task<TResult> ExecuteAsync<TResult>(
        Func<CaPheDbContext, Task<TResult>> operation,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return await ExecuteAsync(operation, _ => true, isolationLevel).ConfigureAwait(false);
    }

    public static async Task<TResult> ExecuteAsync<TResult>(
        Func<CaPheDbContext, Task<TResult>> operation,
        Func<TResult, bool> shouldCommit,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        using var strategyContext = new CaPheDbContext();
        var strategy = strategyContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var context = new CaPheDbContext();
            await using var transaction = await context.Database
                .BeginTransactionAsync(isolationLevel)
                .ConfigureAwait(false);

            try
            {
                var result = await operation(context).ConfigureAwait(false);
                if (shouldCommit(result))
                {
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
                else
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                }

                return result;
            }
            catch
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }).ConfigureAwait(false);
    }

    public static TResult Execute<TResult>(
        Func<CaPheDbContext, TResult> operation,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return Execute(operation, _ => true, isolationLevel);
    }

    public static TResult Execute<TResult>(
        Func<CaPheDbContext, TResult> operation,
        Func<TResult, bool> shouldCommit,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        using var strategyContext = new CaPheDbContext();
        var strategy = strategyContext.Database.CreateExecutionStrategy();

        return strategy.Execute(() =>
        {
            using var context = new CaPheDbContext();
            using var transaction = context.Database.BeginTransaction(isolationLevel);

            try
            {
                var result = operation(context);
                if (shouldCommit(result))
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }

                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        });
    }
}
