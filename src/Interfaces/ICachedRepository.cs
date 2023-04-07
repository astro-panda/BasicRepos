using BasicRepos;

/// <summary>
/// Simply a repository that is expected to maintain a cache of the data. Has 
/// all the functionality of <see cref="IRepository{TEntity}" /> and <see cref="IReadOnlyRepository{TEntity}" />
/// </summary>
/// <typeparam name="TEntity">The <see cref="Type"/> of entity this repository operates with</typeparam>
public interface ICachedRepository<TEntity> : IRepository<TEntity>, IReadOnlyRepository<TEntity>
        where TEntity : class
{
}