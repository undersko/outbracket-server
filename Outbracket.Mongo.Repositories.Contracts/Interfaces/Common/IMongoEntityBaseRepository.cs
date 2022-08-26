using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Outbracket.Mongo.Repositories.Contracts.Interfaces.Common
{
    public interface IMongoEntityBaseRepository<T>
    {
        Task<IAsyncCursor<T>> GetAllAsync();

        Task<IAsyncCursor<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<long> CountAsync();

        Task<long> CountAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetFirstOrDefaultAsync(string id);

        Task<T> CreateAsync(T record);

        Task<T> CreateOrUpdateAsync(T record);

        Task<T> RemoveAsync(string id);
    }
}