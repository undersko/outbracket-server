using Outbracket.Services.Contracts.Models.Common;

namespace Outbracket.Api.Contracts.Responses.Profile
{
    public class Settings
    {
        public CropItem Crop { get; set; }
    }
    
    public class GetUserSettingsApiResponse
    {
        public string UserSettingsId { get; set; }

        public Settings Settings { get; set; }
    }
}