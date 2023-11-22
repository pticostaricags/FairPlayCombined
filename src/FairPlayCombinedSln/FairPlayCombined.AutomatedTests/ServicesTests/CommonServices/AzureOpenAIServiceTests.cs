using Azure.AI.OpenAI;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    //Disabled to avoid incurring in costs
    //[TestClass]
    public class AzureOpenAIServiceTests : ServicesBase
    {
        [TestMethod]
        public async Task Test_ModerateTextContentAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<AzureOpenAIServiceTests>();
            var configuration = configurationBuilder.Build();
            string testSexuallyOffensivePhrase = 
                configuration["testSexuallyOffensivePhrase"] ??
                throw new Exception("'testSexuallyOffensivePhrase' key is not in configuration");
            string testSexuallyExplicityPhrase = configuration["testSexuallyExplicityPhrase"] ?? 
                throw new Exception("'testSexuallyExplicityPhrase' key is not in configuration");
            string testOffensivePhrase = configuration["testOffensivePhrase"] ?? 
                throw new Exception("'testOffensivePhrase' is not in configuration");
            var endpoint = configuration["AzureOpenAI:Endpoint"] ?? 
                throw new Exception("'AzureOpenAI:Endpoint' is not in configuration");
            var key = configuration["AzureOpenAI:Key"] ?? 
                throw new Exception("'AzureOpenAI:Key' is not in configuration");
            OpenAIClient openAIClient=new(endpoint:new(endpoint),
                keyCredential:new Azure.AzureKeyCredential(key));
            AzureOpenAIService azureOpenAIService = new AzureOpenAIService(openAIClient);
            var result = await azureOpenAIService.ModerateTextContentAsync("My name correo is fulainto at somewhere dot com",
                CancellationToken.None);
            Assert.IsTrue(result!.HasPersonalIdentifiableInformation, $"Has PII: {result.PersonalIdentifiableInformation}");
            result = await azureOpenAIService.ModerateTextContentAsync(testOffensivePhrase,
                CancellationToken.None);
            Assert.IsTrue(result!.IsOffensive, $"Has Profanity: {result.Profanity}");
            result = await azureOpenAIService.ModerateTextContentAsync(testSexuallyExplicityPhrase,
                CancellationToken.None);
            Assert.IsTrue(result!.IsSexuallyExplicit, $"Is Sexually Explicit: {result.SexuallyExplicitPhrases}");
            result = await azureOpenAIService.ModerateTextContentAsync(testSexuallyOffensivePhrase,
                CancellationToken.None);
            Assert.IsTrue(result!.IsSexuallySuggestive, $"Is Sexually Suggestive: {result.SexuallySuggestivePhrases}");
        }
    }
}
