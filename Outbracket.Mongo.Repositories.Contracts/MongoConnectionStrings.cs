using Outbracket.Mongo.Repositories.Contracts;

namespace Outbracket.Mongo.Repositories
{
    public class MongoConnectionStrings : IMongoConnectionStrings
    {
        public string Outbracket { get; set; }
    }
}