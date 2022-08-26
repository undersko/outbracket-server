using System.IO;
using System.Threading.Tasks;
using Outbracket.Common.Models;

namespace Outbracket.Common.Services.Blob
{
    public class BlobUtility
    {
        private readonly IBlobUtility _currentUtility;
        
        public BlobUtility(IBlobUtility currentUtility)
        {
            _currentUtility = currentUtility;
        }

        public Task UploadImageAsync(string containerName, Stream stream, string fileName, Crop crop = null)
        {
            return _currentUtility.UploadImageAsync(containerName, stream, fileName, crop);
        }

        public Task<MemoryStream> GetImageAsync(string containerName, string fileName, bool isScaled = false)
        {
            return _currentUtility.GetImageAsync(containerName, fileName, isScaled);
        }

        public async Task DeleteImageAsync(string containerName, string fileName)
        {
            await _currentUtility.DeleteImageAsync(containerName, fileName);
        }
    }
}