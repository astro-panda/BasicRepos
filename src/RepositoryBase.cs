using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BasicRepos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BasicRepos
{

    /// <summary>
    /// Base level repository to supply most generic implementation of an 
    /// <see cref="IRepository{T}"/> to all of its derived repositories
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of entity this repository operates with</typeparam>    
    public abstract class RepositoryBase<T> : IRepository<T>, IReadRepository<T>
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
        /// Creates a new instance of <see cref="RepositoryBase{T}"/>
        /// </summary>
        /// <param name="db">An instance of <see cref="IApplicationDbContext"/> to supply the internal database funcationality</param>
        public RepositoryBase(DbContext db)
        {
            Db = db;
            Entities = Db.Set<T>();
        }

        /// <summary>
        /// Provides an initial query to build an <see cref="IQueryable{T}" /> which can allow for further querying
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate) => Entities.Where(predicate);

        /// <summary>
        /// Allows for the internal entities to be queried with any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        public IQueryable<T> Query() => Entities;

        /// <summary>
        /// Allows for the internal entities to be queried with any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        public IQueryable<K> Query<K>() where K : T => Entities.Cast<K>();

        /// <summary>
        /// Allows for the internal entities to be queried without any type specifications
        /// </summary>
        /// <returns>A facade for the internal <see cref="DbSet{TEntity}"/></returns>
        public IQueryable RawQuery() => Entities;

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
            if(objects != null)
            {
                Entities.AddRange(objects);
                return Db.SaveChangesAsync();
            }

            return Task.CompletedTask;
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
            if(objects.Count() > 1)
            {
                Entities.RemoveRange(objects);
            }
            else
            {
                T entity = objects.FirstOrDefault();

                if (entity == null)
                    return Task.CompletedTask;

                Entities.Remove(entity);
            }
            return Db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates one or more entities and commites those updates
        /// </summary>
        /// <param name="objects">The entities to be updated</param>
        /// <returns>A <see cref="Task"/> representing the work of updating and saving the entities</returns>
        public virtual Task UpdateAsync(params T[] objects) => Db.SaveChangesAsync();
    }
}