using Outbracket.Entities.Account;
using Outbracket.Repositories.Contracts;
using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Repositories.Implementations.Common;

namespace Outbracket.Repositories.Implementations.Account
{
    public class UserInfoRepository: EntityBaseRepository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(Context context) : base(context) { }
    }
}