using System.Threading.Tasks;
using Outbracket.Entities.Account;
using Outbracket.Repositories.Contracts.Interfaces.Common;

namespace Outbracket.Repositories.Contracts.Interfaces.Account
{
    public interface IUserRepository : IEntityBaseRepository<User>
    {
        Task<bool> IsEmailUniqAsync(string email);

        Task<bool> IsUsernameUniqAsync(string username);
    }
}