using System;
using System.Net.Http;
using System.Threading.Tasks;
using Contracts.OtpHandler;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.OtpHandler
{
    public class OtpHandler : IOtpHandler
    {
        private readonly IConfiguration _configuration;
        public OtpHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendOtpCodeAsync(string receptor, string code)
        {
            var apiKey = _configuration["KavehNegar:ApiKey"];
            var template = _configuration["KavehNegar:Template"];
            using var client = new HttpClient
            {
                BaseAddress = new Uri("https://api.kavenegar.com/v1/" +
                                      $"{apiKey}/verify/lookup.json?" +
                                      $"receptor={receptor}&token={code}&template={template}")
            };

            var responseTask = await client.GetAsync(client.BaseAddress);

            return responseTask.IsSuccessStatusCode;
        }
    }
}