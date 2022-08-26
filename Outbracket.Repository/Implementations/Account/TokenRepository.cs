using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Repositories.Implementations.Common;
using System.Threading.Tasks;
using Outbracket.Entities.Account;
using Outbracket.Repositories.Contracts;

namespace Outbracket.Repositories.Implementations.Account
{
    public class TokenRepository : EntityBaseRepository<UserToken>, ITokenRepository
    {
        public TokenRepository(Context context) : base(context) { }
    }
}
