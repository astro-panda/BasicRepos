using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BasicRepos
{
    public interface IKeyedRepository<T, TKey> : IRepository<T>
        where T : class, IKeyedEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Attemptes to delete
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
    }
}