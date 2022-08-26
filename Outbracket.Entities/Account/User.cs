using System;
using System.Collections.Generic;
using Outbracket.Entities.Common;

namespace Outbracket.Entities.Account
{
    public class User: IBaseEntity
    {
        public Guid Id { get; set; }
        
        public string Username { get; set; }
        
        public string Email { get; set; }
        
        public string PasswordHash { get; set; }
        
        public string PasswordSalt { get; set; }
        
        public bool EmailConfirmed { get; set; }
        
        public UserInfo UserInfo { get; set; }
        
        public ICollection<Role> Roles { get; set; }
        
        public ICollection<UserToken> UserTokens { get; set; }
    }
}
