using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Outbracket.Services.Contracts.Interfaces.Account;
using Outbracket.Services.Contracts.Models.Account;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Outbracket.Enums.DbDictionaries;

namespace Outbracket.Services.Implementations.Account
{
    public class AuthService : IAuthService
    {
        private readonly string _jwtSecret;
        private readonly int _jwtLifespan;
        private readonly int _refreshTokenLifespan;
        private readonly int _emailConfirmationTokenLifespan;
        private readonly int _resetPasswordTokenLifespan;
        
        public AuthService(string jwtSecret, int jwtLifespan, int refreshTokenLifespan, int emailConfirmationTokenLifespan, int resetPasswordTokenLifespan)
        {
            _jwtSecret = jwtSecret;
            _jwtLifespan = jwtLifespan;
            _refreshTokenLifespan = refreshTokenLifespan;
            _emailConfirmationTokenLifespan = emailConfirmationTokenLifespan;
            _resetPasswordTokenLifespan = resetPasswordTokenLifespan;
        }
        public UserToken GenerateResetPasswordToken(Guid userId, string ipAddress)
        {
            return GenerateToken(userId, ipAddress, UserTokenType.ResetPasswordToken, _resetPasswordTokenLifespan);
        }
        public UserToken GenerateConfirmationEmailToken(Guid userId, string ipAddress)
        {
            return GenerateToken(userId, ipAddress, UserTokenType.EmailConfirmationToken, _emailConfirmationTokenLifespan);
        }
        public AuthData GetAuthData(Guid userId, IEnumerable<string> roles, string ipAddress)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(_jwtLifespan);
            var jwtToken = GenerateJwtToken(userId, roles, expirationTime);
            var refreshToken = GenerateToken(userId, ipAddress, UserTokenType.RefreshToken, _refreshTokenLifespan);
            return new AuthData
            {
                Token = jwtToken,
                TokenExpirationTime = expirationTime,
                RefreshToken = refreshToken,
                Id = userId.ToString(),
                Roles = roles
            };
        }
        private string GenerateJwtToken(Guid id, IEnumerable<string> roles, DateTime expirationTime)
        {
            var claims = new List<Claim> {new (ClaimTypes.Name, id.ToString())};
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
        private UserToken GenerateToken(Guid userId, string ipAddress, UserTokenType userTokenType, int tokenLifespan)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);
            token = token.Replace("%", "");
            return new UserToken
            {
                Token = token, 
                UserId = userId,
                Type = userTokenType,
                Expires = DateTime.UtcNow.AddSeconds(tokenLifespan),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}
