using Microsoft.EntityFrameworkCore;

namespace BasicRepos
{
    public class DefaultRepository<TEntity, TContext> : RepositoryBase<TEntity> 
        where TEntity : class
        where TContext : DbContext
    {
        public DefaultRepository(TContext db) : base(db)
        {
        }
    }
}
