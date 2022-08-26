namespace Outbracket.Common.Services.Blob.Models
{
    public class S3AccountOptions
    {
        public string AwsKey { get; set; }

        public string AwsSecretKey { get; set; }

        public string BucketRegion { get; set; }
    }
}