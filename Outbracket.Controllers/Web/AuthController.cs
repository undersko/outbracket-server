using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Outbracket.Api.Contracts.Requests.Account;
using Outbracket.Api.Contracts.Responses;
using Outbracket.Globalization;
using Outbracket.Helpers;
using Outbracket.Services.Contracts.Exceptions;
using Outbracket.Services.Contracts.Interfaces.Account;
using Outbracket.Services.Contracts.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Outbracket.Api.Contracts.Responses.Account;
using Outbracket.Common.Services.Notifier;
using Outbracket.Common.Services.Notifier.Email;
using Outbracket.Enums.DbDictionaries;

namespace Outbracket.Controllers.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IUserService _userService;
        
        private readonly IUserInfoService _userInfoService;

        private readonly IAuthService _authService;

        private readonly ITokenService _tokenService;

        private readonly IEmailSender _emailSender;

        public AuthController(IUserService userService, IUserInfoService userInfoService, IAuthService authService, ITokenService tokenService, IEmailSender emailSender)
        {
            _userService = userService;
            _userInfoService = userInfoService;
            _authService = authService;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<Response> LoginPost([FromBody]LoginUserApiRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new ValidationException(new [] {ValidationErrors.UsernameOrEmailIsInvalid});
            }
            if (!user.EmailConfirmed)
            {
                throw new ValidationException(new [] {ValidationErrors.EmailIsNotConfirmed});
            }

            var passwordValid = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!passwordValid)
            {
                throw new ValidationException(new[] { ValidationErrors.UsernameOrEmailIsInvalid });
            }
            var authData = _authService.GetAuthData(user.Id, user.Roles, GetIpAddress());
            var existedRefreshToken = await _tokenService.GetActiveUserTokenAsync(user.Id, UserTokenType.RefreshToken);
            if (existedRefreshToken != null)
            {
                await _tokenService.RefreshToken(existedRefreshToken.Id, authData.RefreshToken);
            }
            else
            {
                await _tokenService.AddToken(authData.RefreshToken);
            }
            SetTokenCookie(authData.RefreshToken.Token, authData.RefreshToken.Expires);

            return Success(ToAuthDataApiResponse(authData));
        }

        [HttpPost("refresh-token")]
        public async Task<Response> RefreshTokenPost()
        {
            var refreshToken = HttpUtility.UrlDecode(Request.Cookies["refreshToken"]);
            var existedRefreshToken = await _tokenService.GetTokenAsync(refreshToken, UserTokenType.RefreshToken);
            if (existedRefreshToken == null || !existedRefreshToken.IsActive)
            {
                SetTokenCookie(string.Empty, null);
                throw new BusinessException(ValidationErrors.RefreshTokenIsInvalid.Item2);
            }

            var user = await _userService.GetByIdAsync(existedRefreshToken.UserId);
            var authData = _authService.GetAuthData(user.Id, user.Roles, GetIpAddress());
            await _tokenService.RefreshToken(existedRefreshToken.Id, authData.RefreshToken);
            SetTokenCookie(authData.RefreshToken.Token, authData.RefreshToken.Expires);

            return Success(ToAuthDataApiResponse(authData));
        }

        [HttpPost("revoke-token")]
        public async Task<Response> RevokeTokenPost()
        {
            var refreshToken = HttpUtility.UrlDecode(Request.Cookies["refreshToken"]);
            var existedRefreshToken = await _tokenService.GetTokenAsync(refreshToken, UserTokenType.RefreshToken);
            if (existedRefreshToken == null)
            {
                SetTokenCookie(string.Empty, null);
                throw new BusinessException(ValidationErrors.RefreshTokenIsInvalid.Item2);
            }

            if(!existedRefreshToken.IsActive)
            {
                return Success();
            }

            await _tokenService.RevokeToken(existedRefreshToken.Id, GetIpAddress());
            SetTokenCookie(string.Empty, null);

            return Success();
        }

        [HttpPost("register")]
        public async Task<Response> RegisterPost([FromBody]RegisterUserApiRequest request)
        {
            var user = await _userService.AddAsync(new UserCreateModel()
                {Email = request.Email, Password = request.Password, Username = request.Username});
            await _userInfoService.CreateOrUpdateUserInfoAsync(new UserInfoModel()
                {UserId = user.Id});
            var emailConfirmationToken = _authService.GenerateConfirmationEmailToken(user.Id, GetIpAddress());
            var notifier = new Notifier(_emailSender);
            var notificationResults = await notifier.SendRegistrationMessagesAsync(new[]
            {
                new Recipient {Address = user.Email, FullName = user.Username}
            }, $"{Request.Scheme}://{Request.Host}/confirm-email?token={HttpUtility.UrlEncode(emailConfirmationToken.Token, Encoding.UTF8)}");
            if (!notificationResults[0])
            {
                await _userService.RemoveAsync(user.Id);
                throw new BusinessException(Messages.ConfirmationEmailIsNotSent.Item2);
            }
            await _tokenService.AddToken(emailConfirmationToken);
            
            return Success();
        }
        
        [HttpPost("remind-password")]
        public async Task<Response> RemindPasswordPost([FromBody]RemindPasswordApiRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ValidationException(new [] {ValidationErrors.UserDoesntExist});
            }
            var resetPasswordToken = _authService.GenerateResetPasswordToken(user.Id, GetIpAddress());
            var notifier = new Notifier(_emailSender);
            var notificationResults = await notifier.SendRestorePasswordMessagesAsync(new[]
            {
                new Recipient {Address = user.Email, FullName = user.Username}
            }, $"{Request.Scheme}://{Request.Host}/reset-password?token={HttpUtility.UrlEncode(resetPasswordToken.Token, Encoding.UTF8)}");
            if (!notificationResults[0])
            {
                throw new BusinessException(Messages.PasswordResetIsNotSent.Item2);
            }
            await _tokenService.AddToken(resetPasswordToken);
            
            return Success();
        }
        
        [HttpPost("reset-password")]
        public async Task<Response> ResetPasswordPost([FromBody]ResetPasswordApiRequest request)
        {
            var existedResetPasswordToken = await _tokenService.GetTokenAsync(request.Token, UserTokenType.ResetPasswordToken);
            if (existedResetPasswordToken == null || !existedResetPasswordToken.IsActive)
            {
                throw new BusinessException(ValidationErrors.ConfirmationEmailTokenInvalid.Item2);
            }
            var user = await _userService.GetByIdAsync(existedResetPasswordToken.UserId);

            if (user == null)
            {
                throw new BusinessException(ValidationErrors.ConfirmationEmailTokenInvalid.Item2);
            }
            if (!user.EmailConfirmed)
            {
                var emailConfirmationToken = _authService.GenerateConfirmationEmailToken(user.Id, GetIpAddress());
                var notifier = new Notifier(_emailSender);
                var notificationResults = await notifier.SendRegistrationMessagesAsync(new[]
                {
                    new Recipient {Address = user.Email, FullName = user.Username}
                }, $"{Request.Scheme}://{Request.Host}/confirm-email?token={HttpUtility.UrlEncode(emailConfirmationToken.Token, Encoding.UTF8)}");
                
                if (!notificationResults[0])
                {
                    throw new BusinessException(Messages.PasswordResetIsNotSent.Item2);
                }
                
                var existedConfirmEmailToken = await _tokenService.GetActiveUserTokenAsync(user.Id, UserTokenType.EmailConfirmationToken);
                await _tokenService.RevokeToken(existedConfirmEmailToken.Id, GetIpAddress());
                await _tokenService.AddToken(emailConfirmationToken);
            }
            await _userService.ResetPasswordByIdAsync(user.Id, request.Password);
            var authData = _authService.GetAuthData(user.Id, user.Roles, GetIpAddress());
            SetTokenCookie(authData.RefreshToken.Token, authData.RefreshToken.Expires);
            return Success(authData);
        }
        
        [HttpPost("confirm-email")]
        public async Task<Response> ConfirmEmailPost([FromBody]ConfirmEmailApiRequest request)
        {
            var existedConfirmEmailToken = await _tokenService.GetTokenAsync(request.Token, UserTokenType.EmailConfirmationToken);
            if (existedConfirmEmailToken == null || !existedConfirmEmailToken.IsActive)
            {
                throw new BusinessException(ValidationErrors.ConfirmationEmailTokenInvalid.Item2);
            }
            var user = await _userService.GetByIdAsync(existedConfirmEmailToken.UserId);

            if (user == null)
            {
                throw new BusinessException(ValidationErrors.ConfirmationEmailTokenInvalid.Item2);
            }
            await _userService.ActivateUserByIdAsync(user.Id);
            await _tokenService.RevokeToken(existedConfirmEmailToken.Id, GetIpAddress());
            var authData = _authService.GetAuthData(user.Id, user.Roles, GetIpAddress());
            SetTokenCookie(authData.RefreshToken.Token, authData.RefreshToken.Expires);
            return Success(authData);
        }

        private void SetTokenCookie(string token, DateTime? expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires
            };
            Response.Cookies.Append("refreshToken", HttpUtility.UrlEncode(token, Encoding.UTF8), cookieOptions);
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private AuthDataApiResponse ToAuthDataApiResponse(AuthData authData)
        {
            return new()
            {
                Id = authData.Id,
                Roles = authData.Roles.ToArray(),
                Token = authData.Token,
                TokenExpirationTime = authData.TokenExpirationTime,
            };
        }

    }
}