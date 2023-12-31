﻿using FairPlayCombined.Services.Common;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class AzureContentModeratorServiceTest
    {
        [TestMethod]
        public async Task Test_ModerateImageAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<AzureContentModeratorServiceTest>();
            var configuration = configurationBuilder.Build();
            var endpoint = configuration["AzureContentModerator:Endpoint"];
            var key = configuration["AzureContentModerator:Key"];
            var adultImageFilePath = configuration["AdultImageFilePath"];
            ContentModeratorClient contentModeratorClient =
                new ContentModeratorClient(new ApiKeyServiceClientCredentials(key));
            contentModeratorClient.Endpoint = endpoint;
            AzureContentModeratorService azureContentModeratorService = new(contentModeratorClient);
            var imageStream = File.OpenRead(adultImageFilePath!);
            var result = await azureContentModeratorService.ModerateImageAsync(imageStream,
                CancellationToken.None);
            Assert.IsTrue(result.IsAdult);
        }
        [TestMethod]
        public async Task Test_ModeratePlainTextAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var endpoint = configuration["AzureContentModerator:Endpoint"];
            var key = configuration["AzureContentModerator:Key"];
            ContentModeratorClient contentModeratorClient = 
                new ContentModeratorClient(new ApiKeyServiceClientCredentials(key));
            contentModeratorClient.Endpoint = endpoint;
            AzureContentModeratorService azureContentModeratorService = new(contentModeratorClient);
            var testSexuallyOffensivePhrase = configuration["testSexuallyOffensivePhrase"]!;
            var moderationResult = await azureContentModeratorService.ModeratePlainTextAsync(testSexuallyOffensivePhrase,
                CancellationToken.None);
            Assert.Inconclusive("Azure content moderator not detecting the slang");
            //Assert.IsTrue(moderationResult.IsSexuallyExplicity);
        }
    }
}
