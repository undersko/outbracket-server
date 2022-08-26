using Microsoft.AspNetCore.Http;

namespace Outbracket.Api.Contracts.Requests.Image
{
    public class UploadImageApiRequest
    {
        public IFormFile Image { get; set; }

        public string ContainerName { get; set; }
    }
}