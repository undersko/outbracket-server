using System.IO;
using System.Threading.Tasks;
using Outbracket.Common.Models;

namespace Outbracket.Common.Services.Blob
{
    public interface IBlobUtility
    {
        Task UploadImageAsync(string containerName, Stream stream, string fileName, Crop crop = null);

        // Task<List<IListBlobItem>> GetScaledImageListAsync(string containerName, int fromIndex, int maxCount);

        Task<MemoryStream> GetImageAsync(string containerName, string fileName, bool isScaled = false);

        Task DeleteImageAsync(string containerName, string fileName);
    }
}