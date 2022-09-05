using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Contracts.FileHandler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Amazon;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.FileHandler
{
    public class AvatarHandler : IAvatarHandler
    {
        private readonly IConfiguration _configuration;

        public AvatarHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> SaveFileAsync(IFormFile avatar)
        {
            var imageName = Guid.NewGuid() + ".png";
            var res = await ConnectToAwsAsync(avatar, imageName);
            return imageName;
        }

        private async Task<PutObjectResponse> ConnectToAwsAsync(IFormFile file, string imageName)
        {
            var accessKey = _configuration["Aws3:AccessKey"];
            var secretKey = _configuration["Aws3:SecretKey"];
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1,
                ServiceURL = _configuration["Aws3:APIEndpoint"],
                ForcePathStyle = true
            };

            var amazonS3Client = new AmazonS3Client(accessKey, secretKey, config);
            await using var newMemoryStream = new MemoryStream();
            await file.CopyToAsync(newMemoryStream);
            var putObjectRequest = new PutObjectRequest()
            {
                InputStream = newMemoryStream,
                Key = imageName,
                BucketName = "api-kenaret"
            };

            return await amazonS3Client.PutObjectAsync(putObjectRequest);
        }


    }
}