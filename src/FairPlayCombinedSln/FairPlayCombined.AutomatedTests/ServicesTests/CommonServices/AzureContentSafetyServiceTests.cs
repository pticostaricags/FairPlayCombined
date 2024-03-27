using Azure;
using Azure.AI.ContentSafety;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class AzureContentSafetyServiceTests : ServicesBase
    {
        [TestMethod]
        public async Task Test_AanalyzeTextAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var endpoint = configuration["AzureContentSafety:Endpoint"]!;
            var key = configuration["AzureContentSafety:Key"]!;
            var testSexuallyOffensivePhrase = configuration["testSexuallyOffensivePhrase"]!;
            ContentSafetyClient contentSafetyClient = new(new Uri(endpoint),
                new AzureKeyCredential(key));
            AzureContentSafetyService azureContentSafetyService = new(contentSafetyClient);
            var result = await azureContentSafetyService.AnalyzeTextAsync(testSexuallyOffensivePhrase,
                CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_AnalyzeImageAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var endpoint = configuration["AzureContentSafety:Endpoint"]!;
            var key = configuration["AzureContentSafety:Key"]!;
            var adultImageFilePath = configuration["AdultImageFilePath"]!;
            var fileBytes = await File.ReadAllBytesAsync(adultImageFilePath);
            ContentSafetyClient contentSafetyClient = new(new Uri(endpoint),
                new AzureKeyCredential(key));
            AzureContentSafetyService azureContentSafetyService = new(contentSafetyClient);
            var result = await azureContentSafetyService.AnalyzeImageAsync(fileBytes,
                CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsAdult);
        }
    }
}
