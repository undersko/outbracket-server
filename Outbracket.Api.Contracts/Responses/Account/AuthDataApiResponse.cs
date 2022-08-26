using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Api.Contracts.Responses.Account
{
    public class AuthDataApiResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpirationTime { get; set; }
        public string Id { get; set; }
        public string[] Roles { get; set; }
    }
}
