using Outbracket.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Entities.Account
{
    public class Role : IBaseDictionary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
