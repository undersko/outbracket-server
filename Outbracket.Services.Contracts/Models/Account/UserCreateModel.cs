using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Services.Contracts.Models.Account
{
    public class UserCreateModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
