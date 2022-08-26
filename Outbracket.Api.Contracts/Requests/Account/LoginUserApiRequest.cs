using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Api.Contracts.Requests.Account
{
    public class LoginUserApiRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
