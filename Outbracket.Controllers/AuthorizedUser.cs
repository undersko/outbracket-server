using System;
using System.Collections.Generic;

namespace Outbracket.Controllers
{
    public class AuthorizedUser
    {
        public Guid Id { get; set; }
        
        public IEnumerable<string> Roles { get; set; }
    }
}