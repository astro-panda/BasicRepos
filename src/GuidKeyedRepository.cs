using Microsoft.EntityFrameworkCore;
using System;

namespace BasicRepos
{
    public class GuidKeyedRepository<T> : KeyedRepositoryBase<T, Guid> where T : class, IKeyedEntity<Guid>
    {
        public GuidKeyedRepository(DbContext db) : base(db) { }
    }
}
