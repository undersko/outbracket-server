using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Repositories.Implementations.Common;
using System.Threading.Tasks;
using Outbracket.Entities.Account;
using Outbracket.Repositories.Contracts;

namespace Outbracket.Repositories.Implementations.Account
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(Context context) : base(context) { }

        public async Task<bool> IsEmailUniqAsync(string email)
        {
            var user = await GetFirstOrDefaultAsync(u => u.Email == email);
            return user == null;
        }

        public async Task<bool> IsUsernameUniqAsync(string username)
        {
            var user = await GetFirstOrDefaultAsync(u => u.Username == username);
            return user == null;
        }
    }
}
