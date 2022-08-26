using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Api.Contracts.Requests.Account
{
    public class ConfirmEmailApiRequest
    {
        public string Token { get; set; }
    }
}
