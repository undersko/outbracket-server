using System;
using Outbracket.Entities.Common;
using Outbracket.Entities.Dictionaries;

namespace Outbracket.Entities.Account
{
    public class UserInfo : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Bio { get; set; }

        public Guid? ImageId { get; set; }

        public int? CountryId { get; set; }
        
        public string? UserSettingsId { get; set; }
        
        public Country Country { get; set; }
        
        public Guid UserId { get; set; }
        
        public User User { get; set; }
    }
}