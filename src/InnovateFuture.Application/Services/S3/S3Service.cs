using Amazon.S3.Model;
using Amazon.S3;
using InnovateFuture.Domain.Entities;
using Microsoft.Extensions.Options; // Add this
using Amazon;

namespace InnovateFuture.Application.Services.S3
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IOptions<AwsSettings> awsOptions) // ✅ Use IOptions<AwsSettings>
        {
            var settings = awsOptions.Value; // Get actual settings

            _s3Client = new AmazonS3Client(settings.AccessKey, settings.SecretKey, RegionEndpoint.GetBySystemName(settings.Region));
            _bucketName = settings.BucketName;
        }

        public async Task<string> UploadAvatarAsync(Stream fileStream, string fileName)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"avatars/{Guid.NewGuid()}_{fileName}",
                    InputStream = fileStream,
                    ContentType = "image/jpeg",
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await _s3Client.PutObjectAsync(request);

                return $"https://{_bucketName}.s3.amazonaws.com/{request.Key}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                throw;
            }
        }
    }
}
