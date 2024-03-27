using FairPlayCombined.Models.AzureOpenAI;
using FairPlayCombined.Models.OpenAI;
using FairPlayCombined.Services.AI;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class SemanticKernelServiceTests : ServicesBase
    {
        private const string VideoCaptions = "Speaker #1: Is Blazer good for applications that handle millions of records\r\n\r\nSpeaker #1: of data?\r\n\r\nSpeaker #1: Yes it is, especially if you use best practices such\r\n\r\nSpeaker #1: as pagination and in the case of Entity Framework code,\r\n\r\nSpeaker #1: the disabling of the changed tracker when you are going\r\n\r\nSpeaker #1: to retrieve data that is not going to be modified.\r\n\r\nSpeaker #1: As you can see in this example I am showing\r\n\r\nSpeaker #1: a list of records from a table that has 1,000,000\r\n\r\nSpeaker #1: records.\r\n\r\nSpeaker #1: The average duration for the retrieval is around 25 milliseconds.\r\n\r\nSpeaker #1: O Yes, Racer is excellent for alications that handle millions\r\n\r\nSpeaker #1: of records of data, esecially if you use best ractices.";
        
        [TestMethod]
        public async Task Test_TranslateTextAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var endpoint = configuration["AzureOpenAI:Endpoint"] ??
                throw new Exception("'AzureOpenAI:Endpoint' is not in configuration");
            var key = configuration["AzureOpenAI:Key"] ??
                throw new Exception("'AzureOpenAI:Key' is not in configuration");
            var deploymentName = configuration["AzureOpenAI:DeploymentName"] ??
                throw new Exception("'AzureOpenAI:DeploymentName' is not in configuration");
            AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration = new()
            {
                DeploymentName = deploymentName,
                Endpoint = endpoint,
                Key = key
            };
            var openAIKey = configuration["OpenAI:Key"] ??
                throw new Exception("'OpenAI:Key' is not in configuration");
            OpenAIServiceConfiguration openAIServiceConfiguration = new()
            {
                Key = openAIKey
            };
            SemanticKernelService semanticKernelService = new(azureOpenAIServiceConfiguration,
                openAIServiceConfiguration);
            var result = await semanticKernelService.TranslateTextAsync("This is a test",
                fromLanguage: "en-US", toLanguage: "es-CR",
                cancellationToken: CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_CreateVideoDailyPostsAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var endpoint = configuration["AzureOpenAI:Endpoint"] ??
                throw new Exception("'AzureOpenAI:Endpoint' is not in configuration");
            var key = configuration["AzureOpenAI:Key"] ??
                throw new Exception("'AzureOpenAI:Key' is not in configuration");
            var deploymentName = configuration["AzureOpenAI:DeploymentName"] ??
                throw new Exception("'AzureOpenAI:DeploymentName' is not in configuration");
            AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration = new()
            {
                DeploymentName = deploymentName,
                Endpoint = endpoint,
                Key = key
            };
            var openAIKey = configuration["OpenAI:Key"] ??
                throw new Exception("'OpenAI:Key' is not in configuration");
            OpenAIServiceConfiguration openAIServiceConfiguration = new()
            {
                Key = openAIKey
            };
            SemanticKernelService semanticKernelService = new(azureOpenAIServiceConfiguration,
                openAIServiceConfiguration);
            var result = await semanticKernelService.CreateVideoDailyPostsAsync(
                videoDescription: "Is Blazor Good For Applications That Handle Millions Of Records Of Data",
                videoEnglishCaptions:VideoCaptions, CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
