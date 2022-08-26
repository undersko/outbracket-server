using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Outbracket.Services.Contracts.Models.Account;
using Outbracket.Services.Contracts.Models.Common;

namespace Outbracket.Services.Contracts.Interfaces.Account
{
    public interface IUserInfoService
    {
        Task<UserInfoModel> GetByIdAsync(Guid id);

        Task<UserInfoModel> GetByUserIdAsync(Guid userId);
        
        Task CreateOrUpdateUserInfoAsync(UserInfoModel userInfo);

        Task UploadUserLogoAsync(UserInfoModel userInfo, Stream stream, CropItem crop);

        Task DeleteUserLogoAsync(Guid userInfoId);

        Task<UserSettingsModel> GetUserSettingsAsync(string userSettingsId);
    }
}