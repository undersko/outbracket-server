using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Services.Contracts.Models.Account
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
