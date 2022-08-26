using System.Collections.Generic;
using Outbracket.Entities.Common;

namespace Outbracket.Entities.Account
{
    public class UserTokenType : IBaseDictionary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserToken> Tokens { get; set; }
    }
}