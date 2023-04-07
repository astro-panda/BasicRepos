using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BasicRepos
{

    /// <summary>
    /// Base level repository to supply most generic implementation of an 
    /// <see cref="IRepository{T}"/> to all of its derived repositories
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of entity this repository operates with</typeparam>    
    public abstract class RepositoryBase<T> : IRepository<T>, IReadOnlyRepository<T>
            where T : class
    {
        /// <summary>
        /// Internal <see cref="DbContext"/>
        /// </summary>
        protected DbContext Db;

        /// <summary>
        /// Set of operating entities
        /// </summary>
        protected DbSet<T> Entities;


        /// <summary>
        /// Creates a new instance of <see cref="RepositoryBase{T}"/>
        /// </summary>
        /// <param name="db">An instance of <see cref="DbContext"/> to supply the internal database functionality</param>
        public RepositoryBase(DbContext db)
        {
            Db = db;
            Entities = Db.Set<T>();
        }


        public virtual IQueryable<T> Query(Expression<Func<T, bool>> predicate) => Entities.Where(predicate);

        public virtual IQueryable<T> Query() => Entities;

        public virtual IQueryable<K> Query<K>() where K : T => Entities.Cast<K>();

        public virtual IQueryable RawQuery() => Entities;

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => Entities.FirstOrDefaultAsync(predicate, cancellationToken);


        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
             => await Entities.ToListAsync(cancellationToken);


        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await Entities.Where(predicate).ToListAsync(cancellationToken);


        public virtual Task<bool> Exists(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => Entities.AnyAsync(predicate, cancellationToken);


        public virtual Task AddAsync(IEnumerable<T> objects, CancellationToken cancellationToken = default)
        {
            if(objects != null && objects.Any())
            {
                Entities.AddRange(objects);
                return Db.SaveChangesAsync(cancellationToken);
            }

            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(IEnumerable<T> objects, CancellationToken cancellationToken = default)
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
            return Db.SaveChangesAsync(cancellationToken);
        }
     
        public virtual Task UpdateAsync(IEnumerable<T> objects, CancellationToken cancellationToken = default)
        {
            if (objects is null || objects.Any() == false)
                return Task.CompletedTask;

            Entities.UpdateRange(objects);
            return Db.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is null)
                return;

            await Entities.AddAsync(entity);
            await Db.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is null)
                return Task.CompletedTask;

            Entities.Update(entity);
            return Db.SaveChangesAsync();
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is null)
                return Task.CompletedTask;

            Entities.Remove(entity);
            return Db.SaveChangesAsync();
        }

        public Task ApplyChanges(CancellationToken cancellationToken = default)
        {
            return Db.SaveChangesAsync(); 
        }
    }
}