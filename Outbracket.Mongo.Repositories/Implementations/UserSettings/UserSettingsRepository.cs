using Outbracket.Mongo.Entities.UserSettings;
using Outbracket.Mongo.Repositories.Contracts;
using Outbracket.Mongo.Repositories.Contracts.Interfaces.UserSettings;
using Outbracket.Mongo.Repositories.Implementations.Common;

namespace Outbracket.Mongo.Repositories.Implementations.UserSettings
{
    public class UserSettingsRepository : MongoEntityBaseRepository<Entities.UserSettings.UserSettings>, IUserSettingsRepository
    {
        public UserSettingsRepository(IMongoConnectionStrings settings): base(settings, "UserSettings") {}
    }
}