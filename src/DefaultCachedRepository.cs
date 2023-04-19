using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BasicRepos;

public class DefaultCachedRepository<TEntity> : ICachedRepository<TEntity> where TEntity : class
{
    private readonly IRepository<TEntity> _innerRepository;
    protected IEnumerable<TEntity> cachedEntities { get; set; } = new List<TEntity>();

    public DefaultCachedRepository(IRepository<TEntity> innerRepository)
    {
        _innerRepository = innerRepository;
    }    

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        if (cachedEntities.Any() == false)
            await RefreshCache(cancellationToken);

        return cachedEntities.Where(predicate.Compile());
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (cachedEntities.Any() == false)
            await RefreshCache(cancellationToken);

        return cachedEntities;
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        if (cachedEntities.Any() == false)
            await RefreshCache(cancellationToken);

        return cachedEntities.FirstOrDefault(predicate.Compile());
    }

    public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        if (cachedEntities.Any() == false)
            await RefreshCache(cancellationToken);

        return cachedEntities.Any(predicate.Compile());
    }

    public async Task RefreshCache(CancellationToken cancellationToken = default)
    {
        cachedEntities = await _innerRepository.GetAllAsync(cancellationToken);
    }

    public virtual Task AddAsync(IEnumerable<TEntity> objects, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task UpdateAsync(IEnumerable<TEntity> objects, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task DeleteAsync(IEnumerable<TEntity> objects, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task ApplyChanges(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual IQueryable<TEntity> Query()
    {
        throw new NotImplementedException();
    }

    public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
    {
        if(cachedEntities.Any())
            return cachedEntities.Where(predicate.Compile()).AsQueryable();

        return _innerRepository.Query(predicate);
    }

    public virtual IQueryable<K> Query<K>() where K : TEntity
    {
        if(cachedEntities.Any())
            return (cachedEntities as IEnumerable<K>).AsQueryable<K>();

        return _innerRepository.Query<K>();
    }

    public virtual IQueryable RawQuery()
    {
        if(cachedEntities.Any())
            return cachedEntities.AsQueryable();

        return _innerRepository.RawQuery();
    }
}