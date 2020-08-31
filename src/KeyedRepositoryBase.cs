

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AstroPanda.Data 
{
    public abstract class KeyedRepositoryBase<T, TKey> : RepositoryBase<T>, IKeyedRepository<T, TKey>
        where T : class, IKeyedEntity<TKey>
        where TKey : IEquatable<TKey>
    {

        public KeyedRepositoryBase(DbContext db) : base(db) { }

        /// <summary>
        /// Attemptes to delete
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(params TKey[] ids)
        {
            var items = await GetAllAsync(x => ids.Contains(x.Id));
            await DeleteAsync(items);
        }

        /// <summary>
        /// Checks for the existence of an entity by key
        /// </summary>
        /// <param name="id">The identifying key of the target entity</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will be <c>true</c> if exists, otherwise <c>false</c></returns>
        public virtual Task<bool> Exists(TKey id) => Exists(x => x.Id.Equals(id));

        /// <summary>
        /// Checks for the existence of a set of entities by key.
        /// 
        /// <para>
        /// Internally should only return <c>true</c> if ALL
        /// keys have a corresponding entity
        /// </para>
        /// </summary>
        /// <param name="ids">The identifying key of the target entity</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will be <c>true</c> if exists, otherwise <c>false</c></returns>
        public virtual async Task<bool> Exists(params TKey[] ids) => await Entities.Where(x => ids.Contains(x.Id)).CountAsync() == ids.Length;

        /// <summary>
        /// Gets an entity by key
        /// </summary>
        /// <param name="id">The identifying key of the entity</param>
        /// <returns>The target entity if exists, otherwise, default</returns>
        public virtual Task<T> GetAsync(TKey id) => GetAsync(x => x.Id.Equals(id));

        /// <summary>
        /// Gets a set of entities by key
        /// </summary>
        /// <param name="ids">The identifying keys of the entities</param>
        /// <returns>The entities that had a corresponding key, otherwise, default</returns>
        public virtual Task<IEnumerable<T>> GetAsync(params TKey[] ids) => GetAllAsync(x => ids.Contains(x.Id));
    }
}