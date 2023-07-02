using GrillBot.Core.Managers.Performance;
using Microsoft.EntityFrameworkCore;

namespace GrillBot.Core.Database.Repository;

public abstract class RepositoryBase<TContext> where TContext : DbContext
{
    protected TContext Context { get; }
    protected ICounterManager CounterManager { get; }

    protected RepositoryBase(TContext context, ICounterManager counterManager)
    {
        Context = context;
        CounterManager = counterManager;
    }

    public Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        => Context.Set<TEntity>().AddAsync(entity).AsTask();

    public Task AddCollectionAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        => Context.Set<TEntity>().AddRangeAsync(entities);

    public void Remove<TEntity>(TEntity entity) where TEntity : class
        => Context.Set<TEntity>().Remove(entity);

    public void RemoveCollection<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        => Context.Set<TEntity>().RemoveRange(entities);

    public async Task<int> CommitAsync()
    {
        using (CounterManager.Create("Repository.Commit"))
        {
            return await Context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsPendingMigrationsAsync()
    {
        using (CounterManager.Create("Repository.Migrations"))
        {
            return (await Context.Database.GetPendingMigrationsAsync()).Any();
        }
    }
}
