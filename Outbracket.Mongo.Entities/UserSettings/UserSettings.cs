using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Outbracket.Mongo.Contracts.Common;

namespace Outbracket.Mongo.Entities.UserSettings
{
    public class UserSettings : IMongoBaseEntity
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        
        public CropItem Crop { get; set; }
    }
}