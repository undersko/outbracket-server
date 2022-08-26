using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Outbracket.Repositories.Contracts.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Outbracket.Entities.Common;
using Outbracket.Repositories.Contracts;

namespace Outbracket.Repositories.Implementations.Common
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
                where T : class, IBaseEntity, new()
    {
        private readonly Context _context;

        public EntityBaseRepository(Context context)
        {
            _context = context;
        }
        public virtual IAsyncEnumerable<T> GetAllAsync()
        {
            return _context.Set<T>().AsAsyncEnumerable();
        }
        
        public virtual IAsyncEnumerable<T> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsAsyncEnumerable();
        }
        
        public virtual IAsyncEnumerable<TO> GetAllAsync<TO>(Expression<Func<T, bool>> predicate, Expression<Func<T, TO>> projectionMethod)
        {
            return _context.Set<T>().Where(predicate).Select(projectionMethod).AsAsyncEnumerable();
        }

        public virtual Task<int> CountAsync()
        {
            return _context.Set<T>().CountAsync();
        }
        
        public virtual IAsyncEnumerable<T> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsAsyncEnumerable();
        }

        public Task<T> GetFirstOrDefaultAsync(Guid id)
        {
            return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).SingleAsync();
        }

        public virtual IAsyncEnumerable<T> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsAsyncEnumerable();
        }

        public virtual void AddAsync(T entity)
        {
            _context.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public virtual async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
