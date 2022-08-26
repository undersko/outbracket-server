using System;
using System.Collections.Generic;
using System.Text;
using Outbracket.Enums.DbDictionaries;

namespace Outbracket.Services.Contracts.Models.Account
{
    public class UserToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserTokenType Type { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
