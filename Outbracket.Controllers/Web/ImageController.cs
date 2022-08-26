using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Outbracket.Api.Contracts.Requests.Image;
using Outbracket.Api.Contracts.Responses;
using Outbracket.Common.Services.Blob;
using Outbracket.Common.Services.Blob.S3;

namespace Outbracket.Controllers.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ApiControllerBase
    {
        private readonly BlobUtility _blobUtility;
        
        public ImageController(IS3BlobUtility s3BlobUtility)
        {
            _blobUtility = new BlobUtility(s3BlobUtility);
        }
        
        [HttpPost("upload")]
        [Authorize]
        public async Task<Response> UploadImage(UploadImageApiRequest request)
        {
            await _blobUtility.UploadImageAsync(request.ContainerName, request.Image.OpenReadStream(), request.Image.Name);
            return Success();
        }
        
        [HttpGet("scaled/{containerName}/{fileName}")]
        public async Task<FileStreamResult> GetScaledImage(string containerName, string fileName)
        {
            return File(await _blobUtility.GetImageAsync(containerName, fileName, true), "application/octet-stream");
        }
        
        [HttpGet("full/{containerName}/{fileName}")]
        public async Task<FileStreamResult> GetFullImage(string containerName, string fileName)
        {
            return File(await _blobUtility.GetImageAsync(containerName, fileName), "application/octet-stream");
        }
    }
}