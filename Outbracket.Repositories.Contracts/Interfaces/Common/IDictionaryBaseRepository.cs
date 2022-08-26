using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Outbracket.Entities.Common;

namespace Outbracket.Repositories.Contracts.Interfaces.Common
{
    public interface IDictionaryBaseRepository<T> where T : class, IBaseDictionary, new()
    {
        IAsyncEnumerable<T> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        IAsyncEnumerable<T> GetAllAsync();
        IAsyncEnumerable<T> GetAllAsync(Expression<Func<T, bool>> predicate);
        IAsyncEnumerable<TO> GetAllAsync<TO>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TO>> projectionMethod);
        Task<int> CountAsync();
        Task<T> GetFirstOrDefaultAsync(int id);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IAsyncEnumerable<T> FindByAsync(Expression<Func<T, bool>> predicate);
        void AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        Task CommitAsync();
    }
}
