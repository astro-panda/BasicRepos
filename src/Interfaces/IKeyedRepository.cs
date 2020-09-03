
using AstroPanda.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AstroPanda.Data
{
    public interface IKeyedRepository<T, TKey> : IRepository<T>, IKeyedReadRepository<T, TKey>
        where T : class, IKeyedEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Attemptes to delete
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteAsync(params TKey[] ids);
    }
}