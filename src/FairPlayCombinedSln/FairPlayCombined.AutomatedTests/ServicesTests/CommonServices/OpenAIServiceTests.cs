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
        public async Task Test_GenerateDallE3UsingSmallPromptAsync()
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

        [TestMethod]
        public async Task Test_GenerateDallE3BasedOnVideoInformationAsync()
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
            OpenAIService openAIService = new(httpClient, openAIServiceConfiguration);
            var result = await openAIService.GenerateDallE3ImageAsync(prompt: "YouTube Thumbnail for video based on the following data. Video Title: Is Blazor Good For Applications That Handle Millions Of Records Of Data. Video Captions: Speaker #1: Is Blazer good for applications that handle millions of records\r\n\r\nSpeaker #1: of data?\r\n\r\nSpeaker #1: Yes it is, especially if you use best practices such\r\n\r\nSpeaker #1: as pagination and in the case of Entity Framework code,\r\n\r\nSpeaker #1: the disabling of the changed tracker when you are going\r\n\r\nSpeaker #1: to retrieve data that is not going to be modified.\r\n\r\nSpeaker #1: As you can see in this example I am showing\r\n\r\nSpeaker #1: a list of records from a table that has 1,000,000\r\n\r\nSpeaker #1: records.\r\n\r\nSpeaker #1: The average duration for the retrieval is around 25 milliseconds.\r\n\r\nSpeaker #1: O Yes, Racer is excellent for alications that handle millions\r\n\r\nSpeaker #1: of records of data, esecially if you use best ractices.", cancellationToken: CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
#endif