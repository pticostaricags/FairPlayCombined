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
    public class SemanticKernelServiceTests: ServicesBase
    {
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
            AzureOpenAIConfiguration azureOpenAIConfiguration = new()
            {
                DeploymentName = deploymentName,
                Endpoint = endpoint,
                Key = key
            };
            SemanticKernelService semanticKernelService = new(azureOpenAIConfiguration);
            var result = await semanticKernelService.TranslateTextAsync("This is a test",
                fromLanguage: "en-US", toLanguage: "es-CR",
                cancellationToken:CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
