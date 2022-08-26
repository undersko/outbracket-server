using Outbracket.Services.Contracts.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Outbracket.Enums.DbDictionaries;

namespace Outbracket.Services.Contracts.Interfaces.Account
{
    public interface ITokenService
    {
        Task<UserToken> GetActiveUserTokenAsync(Guid userId, UserTokenType tokenType);
        Task<UserToken> GetTokenAsync(string token, UserTokenType tokenType);
        Task AddToken(UserToken userToken);
        Task RefreshToken(Guid id, UserToken userToken);
        Task RevokeToken(Guid id, string ipAddress);
    }
}
