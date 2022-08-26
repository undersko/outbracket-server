using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Services.Contracts.Interfaces.Account;
using Outbracket.Services.Contracts.Models.Account;
using System;
using System.Linq;
using System.Threading.Tasks;
using Outbracket.Enums.DbDictionaries;

namespace Outbracket.Services.Implementations.Account
{
    public class TokenService : ITokenService
    {
        private ITokenRepository TokenRepository { get; set; }

        public TokenService(ITokenRepository tokenRepository)
        {
            TokenRepository = tokenRepository;
        }
        
        public async Task<UserToken> GetActiveUserTokenAsync(Guid userId, UserTokenType tokenType)
        {
            var refreshTokens = TokenRepository.GetAllAsync(x => x.UserId == userId && x.TypeId == (int)tokenType, x => ToRefreshTokenModel(x));
            return await refreshTokens.FirstOrDefaultAsync(x => x.IsActive);
        }
        public async Task<UserToken> GetTokenAsync(string token, UserTokenType tokenType)
        {
            var refreshToken = await TokenRepository.GetFirstOrDefaultAsync(x => x.Token == token && x.TypeId == (int)tokenType);
            return ToRefreshTokenModel(refreshToken);
        }
        public async Task AddToken(UserToken userToken)
        {
            userToken.Id = Guid.NewGuid();
            TokenRepository.AddAsync(ToRefreshToken(userToken));

            await TokenRepository.CommitAsync();
        }
        public async Task RefreshToken(Guid id, UserToken userToken)
        {
            var existedRefreshToken = await TokenRepository.GetFirstOrDefaultAsync(id);
            existedRefreshToken.Revoked = DateTime.UtcNow;
            existedRefreshToken.RevokedByIp = userToken.CreatedByIp;
            existedRefreshToken.ReplacedByToken = userToken.Token;
            TokenRepository.Update(existedRefreshToken);

            userToken.Id = Guid.NewGuid();
            TokenRepository.AddAsync(ToRefreshToken(userToken));

            await TokenRepository.CommitAsync();
        }
        public async Task RevokeToken(Guid id, string ipAddress)
        {
            var existedRefreshToken = await TokenRepository.GetFirstOrDefaultAsync(id);
            existedRefreshToken.Revoked = DateTime.UtcNow;
            existedRefreshToken.RevokedByIp = ipAddress;
            TokenRepository.Update(existedRefreshToken);

            await TokenRepository.CommitAsync();
        }
        private static UserToken ToRefreshTokenModel(Entities.Account.UserToken token)
        {
            if (token == null)
            {
                return null;
            }
            return new UserToken()
            {
                Token = token.Token,
                Created = token.Created,
                CreatedByIp = token.CreatedByIp,
                Expires = token.Expires,
                Id = token.Id,
                ReplacedByToken = token.ReplacedByToken,
                Revoked = token.Revoked,
                RevokedByIp = token.RevokedByIp,
                UserId = token.UserId,
                Type = (UserTokenType)token.TypeId
            };
        }

        private static Entities.Account.UserToken ToRefreshToken(UserToken token)
        {
            if (token == null)
            {
                return null;
            }
            return new Entities.Account.UserToken()
            {
                Token = token.Token,
                Created = token.Created,
                CreatedByIp = token.CreatedByIp,
                Expires = token.Expires,
                Id = token.Id,
                ReplacedByToken = token.ReplacedByToken,
                Revoked = token.Revoked,
                RevokedByIp = token.RevokedByIp,
                UserId = token.UserId,
                TypeId = (int)token.Type
            };
        }
    }
}
