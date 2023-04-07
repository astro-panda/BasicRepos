using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BasicRepos;

public class DefaultCachedRepository<TEntity, TContext> : RepositoryBase<TEntity>, ICachedRepository<TEntity>
where TEntity : class
where TContext : DbContext
{
    protected IEnumerable<TEntity> cachedEntities { get; set; } = new List<TEntity>();

    public DefaultCachedRepository(TContext dbFactory) : base(dbFactory)
    {
    }

    public override async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        if (cachedEntities.Any() == false)
            await RefreshCache();

        return cachedEntities.Where(predicate.Compile());
    }

    public override async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (cachedEntities.Any() == false)
            await RefreshCache();

        return cachedEntities;
    }

    private async Task RefreshCache(CancellationToken cancellationToken = default)
    {
        cachedEntities = await base.GetAllAsync(cancellationToken);
    }
}