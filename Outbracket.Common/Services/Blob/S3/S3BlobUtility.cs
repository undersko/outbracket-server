using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ImageMagick;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Outbracket.Common.Constants;
using Outbracket.Common.Models;
using Outbracket.Common.Services.Blob.Models;

namespace Outbracket.Common.Services.Blob.S3
{
    public class S3BlobUtility : IS3BlobUtility
    {
        private readonly AmazonS3Client _client;
        private readonly string _bucket;
        private readonly BlobContainers _containers;

        public S3BlobUtility(S3AccountOptions options, BlobContainers containers, string bucket)
        {
            RegionEndpoint AWSregion;
            if (AWSConstants.RegionsMap.TryGetValue(options.BucketRegion, out AWSregion))
            {
                _client = new AmazonS3Client(options.AwsKey, options.AwsSecretKey, AWSregion);
                _containers = containers;
                _bucket = bucket;
            }
            else
            {
                throw new Exception("AWS configuration Error");
            }
        }

        public async Task UploadImageAsync(string containerName, Stream stream, string fileName, Crop crop = null)
        {
            using (MagickImage image = new MagickImage(stream))
            {
                var putRequest = new PutObjectRequest();
                putRequest.BucketName = _bucket;
                putRequest.ContentType = "image/jpeg";
                await using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.AutoOrient();
                    await memoryStream.WriteAsync(image.ToByteArray(), 0, image.ToByteArray().Length);
                    memoryStream.Position = 0;
                    putRequest.InputStream = memoryStream;
                    putRequest.Key = _containers.FullImagesContainerNameOption + @"/" + containerName + @"/" + fileName;
                    await _client.PutObjectAsync(putRequest);
                }
                await using (MemoryStream memoryStream = new MemoryStream())
                {
                    if (crop == null)
                    {
                        image.Scale(new Percentage(50));
                    }
                    else
                    {
                        image.Scale(crop.ImageWidth, crop.ImageHeight);
                        image.Crop(new MagickGeometry(crop.X, crop.Y, crop.Width, crop.Height));
                    }
                    await memoryStream.WriteAsync(image.ToByteArray(), 0, image.ToByteArray().Length);
                    memoryStream.Position = 0;
                    putRequest.InputStream = memoryStream;
                    putRequest.Key = _containers.ScaledImagesContainerNameOption + @"/" + containerName + @"/" +
                                     fileName;
                    await _client.PutObjectAsync(putRequest);
                }
            }
        }

        // public async Task<List<S3Object>> GetScaledImageListAsync(string containerName, int fromIndex, int maxCount)
        // {
        //     ListObjectsRequest request = new ListObjectsRequest
        //     {
        //         BucketName = _bucket,
        //         Prefix = _containers.ScaledImagesContainerNameOption + @"/" + containerName
        //     };
        //
        //     var objects = await _client.ListObjectsAsync(request);
        //     return objects.S3Objects;
        // }
        
        public async Task<MemoryStream> GetImageAsync(string containerName, string fileName, bool isScaled = false)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _bucket,
                Key = (isScaled ? _containers.ScaledImagesContainerNameOption : _containers.FullImagesContainerNameOption) + @"/" + containerName +  @"/" + fileName
            };
            
            var obj = await _client.GetObjectAsync(request);

            MemoryStream memoryStream = new MemoryStream();
            if (obj != null)
                await obj.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }
        
        public async Task DeleteImageAsync(string containerName, string fileName)
        {
            DeleteObjectRequest deleteScaledRequest = new DeleteObjectRequest
            {
                BucketName = _bucket,
                Key = _containers.ScaledImagesContainerNameOption + @"/" + containerName +  @"/" + fileName
            };
            DeleteObjectRequest deleteFullRequest = new DeleteObjectRequest
            {
                BucketName = _bucket,
                Key = _containers.FullImagesContainerNameOption + @"/" + containerName +  @"/" + fileName
            };
            await _client.DeleteObjectAsync(deleteScaledRequest);
            await _client.DeleteObjectAsync(deleteFullRequest);
        }
    }
}