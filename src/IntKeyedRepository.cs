using Microsoft.EntityFrameworkCore;
using System;

namespace BasicRepos
{
    public class IntKeyedRepository<T> : KeyedRepositoryBase<T, int> where T : class, IKeyedEntity<int>
    {
        public IntKeyedRepository(DbContext db) : base(db) { }
    }
}
