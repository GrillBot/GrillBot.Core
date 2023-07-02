using GrillBot.Core.Managers.Performance;
using Microsoft.EntityFrameworkCore;

namespace GrillBot.Core.Database.Repository;

public abstract class SubRepositoryBase<TContext> where TContext : DbContext
{
    protected TContext Context { get; }
    private ICounterManager CounterManager { get; }

    protected SubRepositoryBase(TContext context, ICounterManager counterManager)
    {
        Context = context;
        CounterManager = counterManager;
    }

    protected IQueryable<TEntity> CreateQuery<TEntity>(IQueryableModel<TEntity> parameters, bool disableTracking = false,
        bool splitQuery = false) where TEntity : class
    {
        var query = Context.Set<TEntity>().AsQueryable();

        query = parameters.SetIncludes(query);
        query = parameters.SetQuery(query);
        query = parameters.SetSort(query);

        if (disableTracking)
            query = query.AsNoTracking();
        if (splitQuery)
            query = query.AsSplitQuery();

        return query;
    }

    protected CounterItem CreateCounter()
        => CounterManager.Create($"Repository.{GetType().Name.Replace("Repository", "")}");
}
