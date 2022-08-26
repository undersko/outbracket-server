using Outbracket.Entities.Account;
using Outbracket.Repositories.Contracts;
using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Repositories.Contracts.Interfaces.Dictionaries;
using Outbracket.Repositories.Implementations.Common;

namespace Outbracket.Repositories.Implementations.Dictionaries
{
    public class RoleRepository : DictionaryBaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(Context context) : base(context) { }
    }
}