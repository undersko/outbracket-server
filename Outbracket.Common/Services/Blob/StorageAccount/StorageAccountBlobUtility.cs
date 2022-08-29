using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Outbracket.Common.Models;
using Outbracket.Common.Services.Blob.Models;

namespace Outbracket.Common.Services.Blob.StorageAccount
{
    public class StorageAccountBlobUtility : IStorageAccountBlobUtility
    {
        private readonly CloudBlobClient _client;
        
        private readonly BlobContainers _containers;

        public StorageAccountBlobUtility(StorageAccountOptions options, BlobContainers containers)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse($"DefaultEndpointsProtocol=https;AccountName={options.StorageAccountNameOption};AccountKey{options.StorageAccountKeyOption};EndpointSuffix=core.windows.net");
            _client = cloudStorageAccount.CreateCloudBlobClient();
            _containers = containers;
        }

        public async Task UploadImageAsync(string containerName, Stream stream, string fileName, Crop crop = null)
        {
            var fullSizeImagesContainer = _client.GetContainerReference(_containers.FullImagesContainerNameOption + @"/" + containerName);
            var scaledImagesContainer = _client.GetContainerReference(_containers.ScaledImagesContainerNameOption + @"/" + containerName);
            await fullSizeImagesContainer.CreateIfNotExistsAsync();
            await fullSizeImagesContainer.SetPermissionsAsync(new BlobContainerPermissions
                {PublicAccess = BlobContainerPublicAccessType.Off});
            
            await scaledImagesContainer.CreateIfNotExistsAsync();
            await scaledImagesContainer.SetPermissionsAsync(new BlobContainerPermissions
                {PublicAccess = BlobContainerPublicAccessType.Off});

            var fullSizeBlockBlob = fullSizeImagesContainer.GetBlockBlobReference(fileName);
            var scaledBlockBlob = scaledImagesContainer.GetBlockBlobReference(fileName);
            
            using (MagickImage image = new MagickImage(stream))
            await using (MemoryStream memoryStream = new MemoryStream())
            {
                image.AutoOrient();
                await memoryStream.WriteAsync(image.ToByteArray(), 0, image.ToByteArray().Length);
                memoryStream.Position = 0;
                await fullSizeBlockBlob.UploadFromStreamAsync(stream);

                if (crop == null)
                {
                    image.Scale(new Percentage(50));
                }
                else
                {
                    if (crop.Unit == "%")
                    {
                        var x = (int) (image.Width * crop.X / 100);
                        var y = (int) (image.Height * crop.Y / 100);
                        image.Crop(
                            new MagickGeometry(x, y, new Percentage((double)crop.Width), new Percentage((double)crop.Height))
                        );
                    }
                    else
                    {
                        image.Scale(crop.ImageWidth, crop.ImageHeight);
                        image.Crop(new MagickGeometry((int)crop.X, (int)crop.Y, (int)crop.Width, (int)crop.Height));
                    }
                }
                await memoryStream.WriteAsync(image.ToByteArray(), 0, image.ToByteArray().Length);
                memoryStream.Position = 0;
                await scaledBlockBlob.UploadFromStreamAsync(stream);
            }
        }

        // public async Task<List<IListBlobItem>> GetScaledImageListAsync(string containerName, int fromIndex, int maxCount)
        // {
        //     var container = _client.GetContainerReference(_containers.ScaledImagesContainerNameOption + @"/" + containerName);
        //
        //     BlobContinuationToken token = null;
        //     List<IListBlobItem> blobs = new List<IListBlobItem>();
        //
        //     do
        //     {
        //         var response = await container.ListBlobsSegmentedAsync(token);
        //         token = response.ContinuationToken;
        //         blobs.AddRange(response.Results);
        //     } while (token != null);
        //
        //     return blobs.GetRange(fromIndex, maxCount);
        // }
        
        public async Task<MemoryStream> GetImageAsync(string containerName, string fileName, bool isScaled = false)
        {
            var container = _client.GetContainerReference((isScaled ? _containers.FullImagesContainerNameOption : _containers.ScaledImagesContainerNameOption) + @"/" + containerName);

            var blob = container.GetBlobReference(fileName);

            MemoryStream stream = new MemoryStream();
            if (blob != null)
                await blob.DownloadToStreamAsync(stream);
            stream.Position = 0;
                
            return stream;
        }
        
        public async Task DeleteImageAsync(string containerName, string fileName)
        {
            var fullSizeImagesContainer = _client.GetContainerReference(_containers.FullImagesContainerNameOption + @"/" + containerName + @"/" + fileName);
            var scaledImagesContainer = _client.GetContainerReference(_containers.ScaledImagesContainerNameOption + @"/" + containerName + @"/" + fileName);
            
            await fullSizeImagesContainer.DeleteAsync();
            await scaledImagesContainer.DeleteAsync();
        }
    }
}