using Microsoft.AspNetCore.Http;
using Outbracket.Services.Contracts.Models.Common;

namespace Outbracket.Api.Contracts.Requests.Account
{
    public class UploadUserLogoApiRequest : UpdateUserInfoApiRequest
    {
        public IFormFile Image { get; set; }
        
        public CropItem Crop { get; set; }
    }
}