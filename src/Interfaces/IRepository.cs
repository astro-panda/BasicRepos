using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BasicRepos
{
    /// <summary>
    /// Base level repository to supply most generic implementation of state changing commands of an 
    /// <see cref="IRepository{T}"/> to all of its derived repositories
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of entity this repository operates with</typeparam>    
    public interface IRepository<T>  : IReadOnlyRepository<T>
        where T : class 
    {
        /// <summary>
        /// Takes a set of new entities and adds them to the <see cref="DbContext"/>
        /// </summary>
        /// <param name="objects">The entities to be added</param>
        /// <returns></returns>
        Task AddAsync(IEnumerable<T> objects, CancellationToken cancellationToken = default);

        /// <summary>
        /// Takes a new entity and adds it to the <see cref="DbContext"/>
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        /// <returns></returns>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates one or more entities and commites those updates
        /// </summary>
        /// <param name="objects">The entities to be updated</param>
        /// <returns>A <see cref="Task"/> representing the work of updating and saving the entities</returns>
        Task UpdateAsync(IEnumerable<T> objects, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entities and commits it as an update
        /// </summary>
        /// <param name="entity">The entity to be updated</param>
        /// <returns>A <see cref="Task"/> representing the work of updating and saving the entities</returns>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Takes a set of entities and removes them from the <see cref="DbContext"/>
        /// </summary>
        /// <param name="objects">The entities to be removed</param>
        /// <returns>A <see cref="Task"/> representing the work of deleting</returns>
        Task DeleteAsync(IEnumerable<T> objects, CancellationToken cancellationToken = default);


        /// <summary>
        /// Takes an entity and removes it from the <see cref="DbContext"/>
        /// </summary>
        /// <param name="entity">The entity to be removed</param>
        /// <returns>A <see cref="Task"/> representing the work of deleting</returns>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a set of entities satisfying a provided expression
        /// </summary>
        /// <param name="predicate">The expression to filter the entities</param>
        /// <returns>A <see cref="Task"/> representing the work of deleting</returns>
        Task DeleteWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Simply saves any changes to all currently tracked entities without needing
        /// to pass in a entity.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task"/> representing the work</returns>
        Task ApplyChanges(CancellationToken cancellationToken = default);
    }
}