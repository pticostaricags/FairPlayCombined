using Azure.Core;
using Azure.Identity;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{

    [TestClass]
    public class AzureVideoIndexerServiceTests : ServicesBase
    {

        [TestMethod]
        public async Task Test_GetVideoStreamingUrlAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"]!;
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"]!;
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"]!;
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"]!;
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"]!;
            var testVideoId = configuration["TestVideoId"]!;
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await this.AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var result = await azureVideoIndexerService.GetVideoStreamingUrlAsync(testVideoId,
                viAccessToken:getAccessToken.AccessToken!,
                CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetVideoThumbnailAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"]!;
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"]!;
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"]!;
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"]!;
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"]!;
            var testVideoId = configuration["TestVideoId"]!;
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await this.AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var indexResponse = await azureVideoIndexerService.GetVideoIndexAsync(
                testVideoId!, getAccessToken.AccessToken!,
                CancellationToken.None);
            Assert.IsNotNull(indexResponse);
            var result = await azureVideoIndexerService.GetVideoThumbnailAsync(testVideoId,
                indexResponse.videos![0].thumbnailId!,
                getAccessToken.AccessToken!, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetSupportedLanguagesAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            _ = configuration["TestVideoId"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await this.AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var result =
            await azureVideoIndexerService
            .GetSupportedLanguagesAsync(getAccessToken!.AccessToken!,
            CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetVideoVTTCaptionsAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            var testVideoId = configuration["TestVideoId"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await this.AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var result =
            await azureVideoIndexerService.GetVideoVTTCaptionsAsync(testVideoId!,
                getAccessToken!.AccessToken!,
                language: "en",
                CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetVideoIndexAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            var testVideoId = configuration["TestVideoId"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await this.AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var result =
            await azureVideoIndexerService.GetVideoIndexAsync(testVideoId!,
                getAccessToken!.AccessToken!,
                CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.videos?.Length);
        }

        [TestMethod]
        public async Task Test_GetAccessTokenForArmAccountAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await this.AuthenticatedToAzureArmAsync();
            var result = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_IndexVideoFromBytesAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            var videoToIndexFullPath = configuration["VideoToIndexFullPath"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string armAccesstoken = await this.AuthenticatedToAzureArmAsync();
            var getAccessTokenResult = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(armAccesstoken, CancellationToken.None);
            Assert.IsNotNull(getAccessTokenResult);
            var fileBytes = File.ReadAllBytes(videoToIndexFullPath!);
            var result = await azureVideoIndexerService.IndexVideoFromBytesAsync(
                new IndexVideoFromBytesFormatModel()
                {
                    FileBytes = fileBytes,
                    Name = $"AT File {Random.Shared.Next(1, 100)}"
                },
                viAccountAccessToken: getAccessTokenResult!.AccessToken!, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_IndexVideoFromUriAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            var videoToIndexUrl = configuration["VideoToIndexUrl"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string armAccesstoken = await this.AuthenticatedToAzureArmAsync();
            var getAccessTokenResult = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(armAccesstoken, CancellationToken.None);
            Assert.IsNotNull(getAccessTokenResult);
            var result = await azureVideoIndexerService.IndexVideoFromUriAsync(
                new AzureVideoIndexerService.IndexVideoFromUriParameters()
                {
                    ArmAccessToken = getAccessTokenResult.AccessToken!,
                    Description = "Test Desc",
                    FileName = "TestFile",
                    Name = $"AT File {Random.Shared.Next(1, 100)}",
                    VideoUri = new Uri(videoToIndexUrl!)
                });
            Assert.IsNotNull(result);
        }

#pragma warning disable CA1822 // Mark members as static
        private async Task<string> AuthenticatedToAzureArmAsync()
#pragma warning restore CA1822 // Mark members as static
        {
            var tokenRequestContext = new TokenRequestContext(["https://management.azure.com/.default"]);
            var tokenRequestResult = await new DefaultAzureCredential().GetTokenAsync(tokenRequestContext, CancellationToken.None);
            return tokenRequestResult.Token;
        }
    }
}
