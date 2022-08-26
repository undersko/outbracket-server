using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Api.Contracts.Requests.Account
{
    public class RegisterUserApiRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
