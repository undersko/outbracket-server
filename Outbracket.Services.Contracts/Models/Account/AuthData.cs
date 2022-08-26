using System;
using System.Collections.Generic;

namespace Outbracket.Services.Contracts.Models.Account
{
    public class AuthData
    {
        public string Token { get; set; }
        public DateTime TokenExpirationTime { get; set; }
        public UserToken RefreshToken { get; set; }
        public string Id { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
