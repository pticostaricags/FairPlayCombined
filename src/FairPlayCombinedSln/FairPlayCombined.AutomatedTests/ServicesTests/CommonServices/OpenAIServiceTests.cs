#if Debug_Enable_Paid_Tests
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class OpenAIServiceTests: ServicesBase
    {
        [TestMethod]
        public async Task Test_GenerateDallE3ImageAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var openAIKey = configuration["OpenAIKey"] ??
                throw new Exception("'OpenAIKey' is not in configuration");
            var generateDall3ImageUrl = configuration["GenerateDall3ImageUrl"] ??
                throw new Exception("'GenerateDall3ImageUrl' is not in configuration");
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                openAIKey);
            OpenAIServiceConfiguration openAIServiceConfiguration = new()
            {
                GenerateDall3ImageUrl = generateDall3ImageUrl
            };
            OpenAIService openAIService =new(httpClient, openAIServiceConfiguration);
            var result = await openAIService.GenerateDallE3ImageAsync(prompt: "logo for a Social Network app named FairPlaySocial", cancellationToken: CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
#endif