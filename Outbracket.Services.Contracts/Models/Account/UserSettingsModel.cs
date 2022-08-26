using System;
using Outbracket.Services.Contracts.Models.Common;

namespace Outbracket.Services.Contracts.Models.Account
{
    public class UserSettingsModel
    {
        public string Id { get; set; }

        public Guid UserId { get; set; }
        
        public CropItem Crop { get; set; }
    }
}