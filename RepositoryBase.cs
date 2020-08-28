using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AstroPanda.Data
{

    /// <summary>
    /// Base level repository to supply most generic implementation of an 
    /// <see cref="IRepository{T, TKey}"/> to all of its derived repositories
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of entity this repository operates with</typeparam>
    /// <typeparam name="TKey">The <see cref="Type"/> of the entity's key</typeparam>
    public abstract class RepositoryBase<T> : IRepository<T>
            where T : class
    {
        /// <summary>
        /// Internal <see cref="IApplicationDbContext"/>
        /// </summary>
        protected DbContext Db;

        /// <summary>
        /// Set of operating entities
        /// </summary>
        protected DbSet<T> Entities;


        /// <summary>
        /// Creates a new instance of <see cref="BaseRepositoryService{T, TKey}"/>
        /// </summary>
        /// <param name="db">An instance of <see cref="IApplicationDbContext"/> to supply the internal database funcationality</param>
        public BaseRepositoryService(DbContext db)
        {
            Db = db;
            Entities = Db.Set<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return Entities.Where(predicate);
        }

        /// <summary>
        /// Allows for the internal entities to be queried with any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        public IQueryable<T> Query()
        {

            return Entities;
        }

        /// <summary>
        /// Allows for the internal entities to be queried with any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        public IQueryable<K> Query<K>() where K : T
        {
            return Entities.Cast<K>();
        }

        /// <summary>
        /// Allows for the internal entities to be queried without any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        public IQueryable RawQuery()
        {

            return Entities;
        }

        /// <summary>
        /// Evaluates an expression of matching criteria to return the first
        /// entity that matches the criteria
        /// </summary>
        /// <param name="predicate">An expression of the matching criteria</param>
        /// <returns>An instance of <see cref="T"/> if it exists, otherwise default</returns>
        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
            => Entities.FirstOrDefaultAsync(predicate);

        /// <summary>
        /// Returns all entities within the set
        /// </summary>
        /// 
        /// <remarks>
        /// As some datasets can be quite massive, please be sure that 
        /// you take care when using this method
        /// </remarks>
        /// <returns>All of the entities within the Entity set</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
             => await Entities.ToListAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
            => await Entities.Where(predicate).ToListAsync();

        /// <summary>
        /// Takes an expression describing the type of entities that should exist and 
        /// evaluates if any entities meet the given criteria.
        /// </summary>
        /// <param name="predicate">An expression of the evaluation criteria</param>
        /// <returns><c>true</c> if any entity matches the criteria, otherwise <c>false</c></returns>
        public Task<bool> Exists(Expression<Func<T, bool>> predicate)
            => Entities.AnyAsync(predicate);

        /// <summary>
        /// Takes a set of new entities and adds them to the <see cref="DbContext"/>
        /// </summary>
        /// <param name="objects">The entities to be added</param>
        /// <returns></returns>
        public virtual Task AddAsync(params T[] objects)
        {
            Entities.AddRange(objects);
            return Db.SaveChangesAsync();
        }

        /// <summary>
        /// Takes a set of entities and removes them from the <see cref="DbContext"/>
        /// </summary>
        /// <param name="objects">The entities to be removed</param>
        /// <returns>A <see cref="Task"/> representing the work of deleting</returns>
        public Task DeleteAsync(params T[] objects) => DeleteAsync(objects);

        /// <summary>
        /// Takes a set of entities and removes them from the <see cref="DbContext"/>
        /// </summary>
        /// <param name="objects">The entities to be removed</param>
        /// <returns>A <see cref="Task"/> representing the work of deleting</returns>
        public Task DeleteAsync(IEnumerable<T> objects)
        {
            Entities.RemoveRange(objects);
            return Db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates one or more entities and commites those updates
        /// </summary>
        /// <param name="objects">The entities to be updated</param>
        /// <returns>A <see cref="Task"/> representing the work of updating and saving the entities</returns>
        public virtual Task UpdateAsync(params T[] objects) => Db.SaveChangesAsync();

        /// <summary>
        /// Attemptes to delete
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public abstract Task DeleteAsync(params TKey[] ids);

        /// <summary>
        /// Checks for the existence of an entity by key
        /// </summary>
        /// <param name="id">The identifying key of the target entity</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will be <c>true</c> if exists, otherwise <c>false</c></returns>
        public abstract Task<bool> Exists(TKey id);

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
        public abstract Task<bool> Exists(TKey[] ids);

        /// <summary>
        /// Gets an entity by key
        /// </summary>
        /// <param name="id">The identifying key of the entity</param>
        /// <returns>The target entity if exists, otherwise, default</returns>
        public abstract Task<T> GetAsync(TKey id);

        /// <summary>
        /// Gets a set of entities by key
        /// </summary>
        /// <param name="ids">The identifying keys of the entities</param>
        /// <returns>The entities that had a corresponding key, otherwise, default</returns>
        public abstract Task<IEnumerable<T>> GetAsync(params TKey[] ids);
    }
}