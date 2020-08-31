using Microsoft.EntityFrameworkCore;
using System;

namespace AstroPanda.Data
{
    public class IntKeyedRepository<T> : KeyedRepositoryBase<T, int> where T : class, IKeyedEntity<int>
    {
        public IntKeyedRepository(DbContext db) : base(db) { }
    }
}
