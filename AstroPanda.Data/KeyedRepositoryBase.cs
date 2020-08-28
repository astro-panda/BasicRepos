

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AstroPanda.Data 
{
    public abstract class KeyedRepositoryBase<T, TKey> : RepositoryBase<T>, IKeyedRepository<T, TKey>
        where T : class
        where TKey : IEquatable<TKey>
    {

        public KeyedRepositoryBase(DbContext db) : base(db)
        {

        }


        public abstract Task DeleteAsync(params TKey[] ids)

        public abstract Task<bool> Exists(TKey id);

        public abstract Task<bool> Exists(TKey[] ids);

        public abstract Task<T> GetAsync(TKey id);

        public abstract Task<IEnumerable<T>> GetAsync(params TKey[] ids);
    }


}