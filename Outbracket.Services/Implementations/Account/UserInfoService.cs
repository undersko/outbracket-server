using System;
using System.IO;
using System.Threading.Tasks;
using Outbracket.Common.Extensions;
using Outbracket.Common.Models;
using Outbracket.Common.Services.Blob;
using Outbracket.Common.Services.Blob.S3;
using Outbracket.Entities.Account;
using Outbracket.Mongo.Entities.UserSettings;
using Outbracket.Mongo.Repositories.Contracts.Interfaces.UserSettings;
using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Services.Contracts.Interfaces.Account;
using Outbracket.Services.Contracts.Models.Account;
using CropItem = Outbracket.Services.Contracts.Models.Common.CropItem;

namespace Outbracket.Services.Implementations.Account
{
    public class UserInfoService : IUserInfoService
    {
        private IUserInfoRepository UserInfoRepository { get; set; }
        private IUserSettingsRepository UserSettingsRepository { get; set; }
        private readonly BlobUtility _blobUtility;

        private string USER_LOGO_CONTAINER = "user_logo";
        
        public UserInfoService(IUserInfoRepository userInfoRepository, IUserSettingsRepository userSettingsRepository, IS3BlobUtility s3BlobUtility)
        {
            UserInfoRepository = userInfoRepository;
            UserSettingsRepository = userSettingsRepository;
            _blobUtility = new BlobUtility(s3BlobUtility);
        }

        public async Task<UserInfoModel> GetByIdAsync(Guid id)
        {
            return ToUserInfoModel(await UserInfoRepository.GetFirstOrDefaultAsync(id));
        }

        public async Task<UserInfoModel> GetByUserIdAsync(Guid userId)
        {
            return ToUserInfoModel(await UserInfoRepository.GetFirstOrDefaultAsync(userInfo => userInfo.UserId == userId, userInfo => userInfo.Country ));
        }

        public async Task CreateOrUpdateUserInfoAsync(UserInfoModel userInfo)
        {
            if (userInfo.Id == null)
            {
                UserInfoRepository.AddAsync(new UserInfo
                {
                    Id = Guid.NewGuid(),
                    UserId = userInfo.UserId,
                    Bio = userInfo.Bio,
                    CountryId = userInfo.Country?.Id,
                    ImageId = userInfo.ImageId
                });
            }
            else
            {
                var existingUserInfo = await UserInfoRepository.GetFirstOrDefaultAsync(userInfo.Id.Value);
                existingUserInfo.Bio = userInfo.Bio;
                existingUserInfo.CountryId = userInfo.Country?.Id;
                existingUserInfo.ImageId = userInfo.ImageId;
                await UserInfoRepository.CommitAsync();
            }
        }

        public async Task UploadUserLogoAsync(UserInfoModel userInfo, Stream stream, CropItem crop)
        {
            if (userInfo.Id == null)
            {
                return;
            }
            var existingUserInfo = await UserInfoRepository.GetFirstOrDefaultAsync(userInfo.Id.Value);

            if (stream == null && existingUserInfo.ImageId != null)
            {
                stream = await _blobUtility.GetImageAsync(USER_LOGO_CONTAINER, existingUserInfo.ImageId.ToString());
            }

            var imageId = Guid.NewGuid();
            await _blobUtility.DeleteImageAsync(USER_LOGO_CONTAINER, existingUserInfo.ImageId.ToString());
            await _blobUtility.UploadImageAsync(USER_LOGO_CONTAINER, stream, imageId.ToString(), ToCrop(crop));
            existingUserInfo.ImageId = imageId;
            var userSettings = await UserSettingsRepository.CreateOrUpdateAsync(ToUserSettings(existingUserInfo.UserSettingsId, existingUserInfo, crop));
            existingUserInfo.UserSettingsId = userSettings.Id;
            await UserInfoRepository.CommitAsync();
        }

        public async Task DeleteUserLogoAsync(Guid userInfoId)
        {
            var existingUserInfo = await UserInfoRepository.GetFirstOrDefaultAsync(userInfoId);
            await _blobUtility.DeleteImageAsync(USER_LOGO_CONTAINER, existingUserInfo.ImageId.ToString());
            var userSettings = await UserSettingsRepository.GetFirstOrDefaultAsync(existingUserInfo.UserSettingsId);
            userSettings.Crop = null;
            await UserSettingsRepository.CreateOrUpdateAsync(userSettings);
            existingUserInfo.ImageId = null;
            await UserInfoRepository.CommitAsync();
        }
        
        public async Task<UserSettingsModel> GetUserSettingsAsync(string userSettingsId)
        {
            return ToUserSettingsModel(await UserSettingsRepository.GetFirstOrDefaultAsync(userSettingsId));
        }
        
        private static UserInfoModel ToUserInfoModel(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            return new UserInfoModel()
            {
                Id = userInfo.Id, Bio = userInfo.Bio, 
                Country = userInfo.Country.ToDictionaryItem(),
                ImageId = userInfo.ImageId,
                UserId = userInfo.UserId,
                UserSettingsId = userInfo.UserSettingsId
            };
        }
        
        private static Crop ToCrop(CropItem crop)
        {
            return new ()
            {
                X = crop.X, 
                Y = crop.Y,
                Aspect = crop.Aspect,
                Height = crop.Height,
                Width = crop.Width,
                Unit = crop.Unit,
                ImageHeight = crop.ImageHeight,
                ImageWidth = crop.ImageWidth
            };
        }

        private static UserSettings ToUserSettings(string? id, UserInfo userInfo, CropItem crop)
        {
            if (userInfo?.Id == null)
            {
                return null;
            }
            return new()
            {
                Crop = new Mongo.Entities.UserSettings.CropItem()
                {
                    Aspect = crop.Aspect,
                    Height = crop.Height,
                    Unit = crop.Unit,
                    Width = crop.Width,
                    X = crop.X,
                    Y = crop.Y
                },
                Id = id,
                UserId = userInfo.UserId
            };
        }
        
        private static UserSettingsModel ToUserSettingsModel(UserSettings userSettings)
        {
            if (userSettings == null)
            {
                return null;
            }
            return new()
            {
                Id = userSettings.Id,
                UserId = userSettings.UserId,
                Crop = userSettings.Crop != null ? new CropItem()
                {
                    Aspect = userSettings.Crop.Aspect,
                    Height = userSettings.Crop.Height,
                    Unit = userSettings.Crop.Unit,
                    Width = userSettings.Crop.Width,
                    X = userSettings.Crop.X,
                    Y = userSettings.Crop.Y
                } : null,
            };
        }
    }
}