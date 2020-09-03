using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroPanda.Data.Interfaces
{
    /// <summary>
    /// A read-only key based repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IKeyedReadRepository<T, TKey>
        where T : class, IKeyedEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets an entity by key
        /// </summary>
        /// <param name="id">The identifying key of the entity</param>
        /// <returns>The target entity if exists, otherwise, default</returns>
        Task<T> GetAsync(TKey id);

        /// <summary>
        /// Gets a set of entities by key
        /// </summary>
        /// <param name="ids">The identifying keys of the entities</param>
        /// <returns>The entities that had a corresponding key, otherwise, default</returns>
        Task<IEnumerable<T>> GetAsync(params TKey[] ids);

        /// <summary>
        /// Checks for the existence of an entity by key
        /// </summary>
        /// <param name="id">The identifying key of the target entity</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will be <c>true</c> if exists, otherwise <c>false</c></returns>
        Task<bool> Exists(TKey id);

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
        Task<bool> Exists(TKey[] ids);
    }
}
