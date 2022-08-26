using Outbracket.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Entities.Account
{
    public class UserToken : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TypeId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public User User { get; set; }
        public UserTokenType Type { get; set; }
    }
}
