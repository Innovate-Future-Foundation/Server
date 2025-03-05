using Amazon.S3.Model;
using Amazon.S3;
using InnovateFuture.Domain.Entities;
using Microsoft.Extensions.Options;
using Amazon;
using InnovateFuture.Application.Exceptions;

namespace InnovateFuture.Application.Services.S3
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IOptions<AWSSettings> awsOptions)
        {
            var settings = awsOptions.Value;

            _s3Client = new AmazonS3Client(settings.AccessKey, settings.SecretKey, RegionEndpoint.GetBySystemName(settings.Region));
            _bucketName = settings.BucketName;
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName,string contentType)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"images/{Guid.NewGuid()}_{fileName}",
                    InputStream = fileStream,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await _s3Client.PutObjectAsync(request);
                
                return $"https://{_bucketName}.s3.amazonaws.com/{request.Key}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                throw new IFExternalServiceException("Fail to upload image.");
            }
        }
    }
}
