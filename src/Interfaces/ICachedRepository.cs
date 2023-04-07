using BasicRepos;
using System.Threading.Tasks;
using System.Threading;

/// <summary>
/// Simply a repository that is expected to maintain a cache of the data. Has 
/// all the functionality of <see cref="IRepository{TEntity}" /> and <see cref="IReadOnlyRepository{TEntity}" />
/// </summary>
/// <typeparam name="TEntity">The <see cref="Type"/> of entity this repository operates with</typeparam>
public interface ICachedRepository<TEntity> : IRepository<TEntity>, IReadOnlyRepository<TEntity>
        where TEntity : class
{
    /// <summary>
    /// Refreshes the internal cache with fresh data from the upstream data source
    /// </summary>
    /// <param name="cancellationToken">A token to cancel operations</param>
    /// <returns>A <see cref="Task"/> representing the work of refreshing the data</returns>
    Task RefreshCache(CancellationToken cancellationToken = default);
}