using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Api.Contracts.Requests.Account
{
    public class ResetPasswordApiRequest
    {
        public string Token { get; set; }

        public string Password { get; set; }
    }
}
