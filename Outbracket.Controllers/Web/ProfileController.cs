using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Outbracket.Api.Contracts.Requests.Account;
using Outbracket.Api.Contracts.Responses;
using Outbracket.Api.Contracts.Responses.Profile;
using Outbracket.Common.Extensions;
using Outbracket.Globalization;
using Outbracket.Services.Contracts.Exceptions;
using Outbracket.Services.Contracts.Interfaces.Account;
using Outbracket.Services.Contracts.Models.Account;

namespace Outbracket.Controllers.Web
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProfileController : ApiControllerBase
    {
        private readonly IUserInfoService _userInfoService;

        private readonly IUserService _userService;
        public ProfileController(IUserInfoService userInfoService, IUserService userService)
        {
            _userInfoService = userInfoService;
            _userService = userService;
        }

        [HttpGet("info")]
        public async Task<Response> GetUserInfo()
        {
            var user = GetUser();
            var userMainInfo = await _userService.GetByIdAsync(user.Id);
            var userInfo = await _userInfoService.GetByUserIdAsync(user.Id);
            return Success(ToUserInfoApiResponse(userMainInfo, userInfo));
        }
        
        [HttpGet("settings/{userSettingsId}")]
        public async Task<Response> GetUserSettings(string userSettingsId)
        {
            var user = GetUser();
            var userMainInfo = await _userService.GetByIdAsync(user.Id);
            var userSettings = await _userInfoService.GetUserSettingsAsync(userSettingsId);
            if (userMainInfo.Id != userSettings.UserId)
            {
                throw new BusinessException(Messages.OperationIsNotPermitted.Item2);
            }
            return Success(ToUserSettingsApiResponse(userSettings));
        }
        
        [HttpPatch("info")]
        public async Task<Response> UpdateUserInfo(UpdateUserInfoApiRequest userInfo)
        {
            await _userInfoService.CreateOrUpdateUserInfoAsync(ToUserInfo(userInfo));
            var user = GetUser();
            var userMainInfo = await _userService.GetByIdAsync(user.Id);
            var newUserInfo = await _userInfoService.GetByUserIdAsync(user.Id);
            return Success(ToUserInfoApiResponse(userMainInfo, newUserInfo));
        }
        
        [HttpPatch("logo")]
        public async Task<Response> UploadUserLogo([FromForm] UploadUserLogoApiRequest userLogo)
        {
            var user = GetUser();
            await _userInfoService.UploadUserLogoAsync(ToUserInfo(userLogo), userLogo.Image?.OpenReadStream(), userLogo.Crop);
            var userMainInfo = await _userService.GetByIdAsync(user.Id);
            var newUserInfo = await _userInfoService.GetByUserIdAsync(user.Id);
            return Success(ToUserInfoApiResponse(userMainInfo, newUserInfo));
        }
        
        [HttpDelete("logo/{userInfoId}")]
        public async Task<Response> DeleteUserLogo(Guid userInfoId)
        {
            var user = GetUser();
            var userInfo = await _userInfoService.GetByIdAsync(userInfoId);
            var userMainInfo = await _userService.GetByIdAsync(user.Id);
            if(user.Id != userInfo.UserId)
            {
                throw new BusinessException(Messages.OperationIsNotPermitted.Item2);
            }

            await _userInfoService.DeleteUserLogoAsync(userInfoId);
            var newUserInfo = await _userInfoService.GetByUserIdAsync(user.Id);
            
            return Success(ToUserInfoApiResponse(userMainInfo, newUserInfo));
        }
        
        private UserInfoModel ToUserInfo(UpdateUserInfoApiRequest userInfo)
        {
            return new()
            {
                Id = userInfo.Id, 
                Bio = userInfo.Bio, 
                Country = userInfo.Country.ToDictionaryItem(), 
                ImageId = userInfo.ImageId
            };
        }
        
        private GetUserInfoApiResponse ToUserInfoApiResponse(UserModel user, UserInfoModel userInfo)
        {
            if (user == null || userInfo == null)
            {
                return null;
            }
            return new()
            {
                Id = userInfo.Id,
                Bio = userInfo.Bio,
                Country = userInfo.Country,
                Email = user.Email,
                Username = user.Username,
                ImageId = userInfo.ImageId,
                UserSettingsId = userInfo.UserSettingsId
            };
        }

        private GetUserSettingsApiResponse ToUserSettingsApiResponse(UserSettingsModel userSettings)
        {
            if (userSettings == null)
            {
                return null;
            }

            return new()
            {
                UserSettingsId = userSettings.Id,
                Settings = new()
                {
                    Crop = userSettings.Crop != null ? new()
                    {
                        X = userSettings.Crop.X,
                        Y = userSettings.Crop.Y,
                        Aspect = userSettings.Crop.Aspect,
                        Width = userSettings.Crop.Width,
                        Height = userSettings.Crop.Height,
                        Unit = userSettings.Crop.Unit
                    } : null
                }
            };
        }
    }
}