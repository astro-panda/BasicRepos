using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AstroPanda.Data
{
    /// <summary>
    /// Base level repository to supply most generic implementation of an 
    /// <see cref="IRepository{T}"/> to all of its derived repositories
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of entity this repository operates with</typeparam>    
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Allows for the internal entities to be queried with any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        IQueryable<T> Query();

        /// <summary>
        /// Allows for the internal entities to be queried with any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        IQueryable<T> Query(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Allows for the internal entities to be queried with any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        IQueryable<K> Query<K>() where K : T;

        /// <summary>
        /// Allows for the internal entities to be queried without any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        IQueryable RawQuery();

        /// <summary>
        /// Returns all entities within the set
        /// </summary>
        /// 
        /// <remarks>
        /// As some datasets can be quite massive, please be sure that 
        /// you take care when using this method
        /// </remarks>
        /// <returns>All of the entities within the Entity set</returns>
        Task<IEnumerable<T>> GetAllAsync();
        
        /// <summary>
        /// </summary>
        ///<returns></returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Evaluates an expression of matching criteria to return the first
        /// entity that matches the criteria
        /// </summary>
        /// <param name="predicate">An expression of the matching criteria</param>
        /// <returns>An instance of <see cref="T"/> if it exists, otherwise default</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Takes a set of new entities and adds them to the <see cref="DbContext"/>
        /// </summary>
        /// <param name="objects">The entities to be added</param>
        /// <returns></returns>
        Task AddAsync(params T[] objects);

        /// <summary>
        /// Updates one or more entities and commites those updates
        /// </summary>
        /// <param name="objects">The entities to be updated</param>
        /// <returns>A <see cref="Task"/> representing the work of updating and saving the entities</returns>
        Task UpdateAsync(params T[] objects);
        
        /// <summary>
        /// Takes a set of entities and removes them from the <see cref="DbContext"/>
        /// </summary>
        /// <param name="objects">The entities to be removed</param>
        /// <returns>A <see cref="Task"/> representing the work of deleting</returns>
        Task DeleteAsync(params T[] objects);

        /// <summary>
        /// Takes a set of entities and removes them from the <see cref="DbContext"/>
        /// </summary>
        /// <param name="objects">The entities to be removed</param>
        /// <returns>A <see cref="Task"/> representing the work of deleting</returns>
        Task DeleteAsync(IEnumerable<T> objects);

        /// <summary>
        /// Takes an expression describing the type of entities that should exist and 
        /// evaluates if any entities meet the given criteria.
        /// </summary>
        /// <param name="predicate">An expression of the evaluation criteria</param>
        /// <returns><c>true</c> if any entity matches the criteria, otherwise <c>false</c></returns>
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
    }
}