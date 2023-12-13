#if Debug_Enable_Paid_Tests
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
    [TestClass]
    public class AzureOpenAIServiceTests : ServicesBase
    {
        [TestMethod]
        public async Task Test_GenerateLinkedInArticleFromVideoCaptionsAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var endpoint = configuration["AzureOpenAI:Endpoint"] ??
                throw new Exception("'AzureOpenAI:Endpoint' is not in configuration");
            var key = configuration["AzureOpenAI:Key"] ??
                throw new Exception("'AzureOpenAI:Key' is not in configuration");
            OpenAIClient openAIClient = new(endpoint: new(endpoint),
                keyCredential: new Azure.AzureKeyCredential(key));
            AzureOpenAIService azureOpenAIService = new AzureOpenAIService(openAIClient);
            var result = await azureOpenAIService.GenerateLinkedInArticleFromVideoCaptionsAsync(
                videoTitle: "Is Blazor Good For Applications That Handle Millions Of Records Of Data",
                videoCaptions: "Speaker #1: Is Blazer good for applications that handle millions of records\r\n\r\nSpeaker #1: of data?\r\n\r\nSpeaker #1: Yes it is, especially if you use best practices such\r\n\r\nSpeaker #1: as pagination and in the case of Entity Framework code,\r\n\r\nSpeaker #1: the disabling of the changed tracker when you are going\r\n\r\nSpeaker #1: to retrieve data that is not going to be modified.\r\n\r\nSpeaker #1: As you can see in this example I am showing\r\n\r\nSpeaker #1: a list of records from a table that has 1,000,000\r\n\r\nSpeaker #1: records.\r\n\r\nSpeaker #1: The average duration for the retrieval is around 25 milliseconds.\r\n\r\nSpeaker #1: O Yes, Racer is excellent for alications that handle millions\r\n\r\nSpeaker #1: of records of data, esecially if you use best ractices.",
                articleMood: AzureOpenAIService.ArticleMood.Hilarious,
                CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_ModerateTextContentAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
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
#endif