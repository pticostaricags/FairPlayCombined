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
            string testSexuallyOffensivePhrase = configuration["testSexuallyOffensivePhrase"];
            string testSexuallyExplicityPhrase = configuration["testSexuallyExplicityPhrase"];
            string testOffensivePhrase = configuration["testOffensivePhrase"];
            var endpoint = configuration["AzureOpenAI:Endpoint"];
            var key = configuration["AzureOpenAI:Key"];
            OpenAIClient openAIClient=new(endpoint:new(endpoint),
                keyCredential:new Azure.AzureKeyCredential(key));
            AzureOpenAIService azureOpenAIService = new AzureOpenAIService(openAIClient);
            var result = await azureOpenAIService.ModerateTextContentAsync("My name correo is fulainto at somewhere dot com",
                CancellationToken.None);
            Assert.IsTrue(result.HasPersonalIdentifiableInformation, $"Has PII: {result.PersonalIdentifiableInformation}");
            result = await azureOpenAIService.ModerateTextContentAsync(testOffensivePhrase,
                CancellationToken.None);
            Assert.IsTrue(result.IsOffensive, $"Has Profanity: {result.Profanity}");
            result = await azureOpenAIService.ModerateTextContentAsync(testSexuallyExplicityPhrase,
                CancellationToken.None);
            Assert.IsTrue(result.IsSexuallyExplicit, $"Is Sexually Explicit: {result.SexuallyExplicitPhrases}");
            result = await azureOpenAIService.ModerateTextContentAsync(testSexuallyOffensivePhrase,
                CancellationToken.None);
            Assert.IsTrue(result.IsSexuallySuggestive, $"Is Sexually Suggestive: {result.SexuallySuggestivePhrases}");
        }
    }
}
