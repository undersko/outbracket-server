using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Outbracket.Mongo.Contracts.Common;
using Outbracket.Mongo.Repositories.Contracts;
using Outbracket.Mongo.Repositories.Contracts.Interfaces.Common;

namespace Outbracket.Mongo.Repositories.Implementations.Common
{
    public class MongoEntityBaseRepository<T> : IMongoEntityBaseRepository<T>
        where T : class, IMongoBaseEntity, new()
    {
        private readonly IMongoCollection<T> _records;
        
        public MongoEntityBaseRepository(IMongoConnectionStrings settings, string collectionName)
        {
            var outbracketClient = new MongoClient(settings.Outbracket);
            var outbracketDatabase = outbracketClient.GetDatabase("Outbracket");
            
            _records = outbracketDatabase.GetCollection<T>(collectionName);
        }
        
        public virtual Task<IAsyncCursor<T>> GetAllAsync()
        {
            return _records.FindAsync(x => true);
        }
        
        public virtual Task<IAsyncCursor<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return _records.FindAsync(predicate);
        }

        public virtual Task<long> CountAsync()
        {
            return _records.CountDocumentsAsync(x => true);
        }
        
        public virtual Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return _records.CountDocumentsAsync(predicate);
        }
        
        public virtual async Task<T> GetFirstOrDefaultAsync(string id)
        {
            var filteredRecords = await _records.FindAsync(x => x.Id == id);
            return filteredRecords.FirstOrDefault();
        }

        public virtual async Task<T> CreateAsync(T record)
        {
            await _records.InsertOneAsync(record);
            return record;
        }
        
        public virtual async Task<T> CreateOrUpdateAsync(T record)
        {
            record.Id ??= ObjectId.GenerateNewId().ToString();
            return await _records.FindOneAndReplaceAsync<T>(x => x.Id == record.Id, record, new FindOneAndReplaceOptions<T>(){ IsUpsert = true, ReturnDocument = ReturnDocument.After });
        }

        public virtual async Task<T> RemoveAsync(string id)
        {
            return await _records.FindOneAndDeleteAsync(x => x.Id == id);
        }
    }
}