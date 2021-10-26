using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicRepos
{
    public class DefaultKeyedRepository<TEntity, TKey, TContext> : KeyedRepositoryBase<TEntity, TKey>
        where TEntity : class, IKeyedEntity<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
    {
        public DefaultKeyedRepository(TContext db) : base(db)
        {
        }
    }
}
