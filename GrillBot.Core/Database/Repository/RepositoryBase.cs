using GrillBot.Core.Managers.Performance;
using Microsoft.EntityFrameworkCore;

namespace GrillBot.Core.Database.Repository;

public abstract class RepositoryBase<TContext> where TContext : DbContext
{
    protected TContext Context { get; }
    private ICounterManager CounterManager { get; }

    protected RepositoryBase(TContext context, ICounterManager counterManager)
    {
        Context = context;
        CounterManager = counterManager;
    }

    protected CounterItem CreateCounter()
        => CounterManager.Create($"Repository.{GetType().Name.Replace("Repository", "")}");
}
