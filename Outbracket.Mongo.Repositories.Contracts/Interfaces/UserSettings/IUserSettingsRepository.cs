using Outbracket.Mongo.Entities.UserSettings;
using Outbracket.Mongo.Repositories.Contracts.Interfaces.Common;

namespace Outbracket.Mongo.Repositories.Contracts.Interfaces.UserSettings
{
    public interface IUserSettingsRepository : IMongoEntityBaseRepository<Entities.UserSettings.UserSettings>
    {
        
    }
}