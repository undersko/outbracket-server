using System;
using Outbracket.Services.Contracts.Models.Common;

namespace Outbracket.Services.Contracts.Models.Account
{
    public class UserInfoModel
    {
        public Guid? Id { get; set; }

        public string Bio { get; set; }

        public Guid? ImageId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string? UserSettingsId { get; set; }

        public DictionaryItem Country { get; set; }
    }
}