using System;
using System.Collections.Generic;
using Outbracket.Services.Contracts.Models.Account;

namespace Outbracket.Services.Contracts.Interfaces.Account
{
    public interface IAuthService
    {
        UserToken GenerateResetPasswordToken(Guid userId, string ipAddress);
        UserToken GenerateConfirmationEmailToken(Guid userId, string ipAddress);
        AuthData GetAuthData(Guid userId, IEnumerable<string> roles, string ipAddress);
    }
}
