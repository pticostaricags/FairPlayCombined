using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.AzureVideoIndexer;
using FairPlayCombined.Models.GoogleAuth;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlayTube;
using Google.Apis.Auth.OAuth2;

namespace FairPlayTube.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        internal static void AddPlatformServices(this WebApplicationBuilder builder,
            GoogleAuthClientSecretInfo googleAuthClientSecretInfo)
        {
            builder.Services.AddTransient<ICultureService, CultureService>();
            builder.Services.AddSingleton<AzureVideoIndexerServiceConfiguration>(sp =>
            {
                var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
                var azureVideoIndexerAccountIdEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY} in database");
                var azureVideoIndexerLocationEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_LOCATION_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_LOCATION_KEY} in database");
                var azureVideoIndexerResourceGroupEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY} in database");
                var azureVideoIndexerResourceNameEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY} in database");
                var azureVideoIndexerSubscriptionIdEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY} in database");
                AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration = new()
                {
                    AccountId = azureVideoIndexerAccountIdEntity.Value,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocationEntity.Value,
                    ResourceGroup = azureVideoIndexerResourceGroupEntity.Value,
                    ResourceName = azureVideoIndexerResourceNameEntity.Value,
                    SubscriptionId = azureVideoIndexerSubscriptionIdEntity.Value
                };
                return azureVideoIndexerServiceConfiguration;
            });
            builder.Services.AddTransient<IAzureVideoIndexerService, AzureVideoIndexerService>(sp =>
            {
                var azureVideoIndexerServiceConfiguration = sp.GetRequiredService<AzureVideoIndexerServiceConfiguration>();
                return new AzureVideoIndexerService(azureVideoIndexerServiceConfiguration,
                    new HttpClient());
            });
            builder.Services.AddTransient<IVideoInfoService, VideoInfoService>();
            builder.Services.AddSingleton<ClientSecrets>(new ClientSecrets()
            {
                ClientId = googleAuthClientSecretInfo.installed!.client_id,
                ClientSecret = googleAuthClientSecretInfo.installed.client_secret
            });
            builder.Services.AddTransient<IYouTubeClientService, YouTubeClientService>();
            builder.Services.AddTransient<IVideoCaptionsService, VideoCaptionsService>();
            builder.Services.AddTransient<IVideoDigitalMarketingPlanService, VideoDigitalMarketingPlanService>();
            builder.Services.AddTransient<IVideoDigitalMarketingDailyPostsService, VideoDigitalMarketingDailyPostsService>();
            builder.Services.AddTransient<IVideoPlanService, VideoPlanService>();
            builder.Services.AddTransient<IPromptGeneratorService, PromptGeneratorService>();
            builder.Services.AddTransient<IVideoWatchTimeService, VideoWatchTimeService>();
            builder.Services.AddTransient<ISupportedLanguageService, SupportedLanguageService>();
            builder.Services.AddTransient<IVideoViewerService, VideoViewerService>();
            builder.Services.AddTransient<IUserMessageService, UserMessageService>();
            builder.Services.AddTransient<IVideoThumbnailService, VideoThumbnailService>();
            builder.Services.AddTransient<IPhotoService, PhotoService>();
            builder.Services.AddTransient<IVideoCommentService, VideoCommentService>();
            builder.Services.AddTransient<IAspNetUsersService, AspNetUsersService>();
            builder.Services.AddTransient<IUserFundService, UserFundService>();
            builder.Services.AddTransient<INewVideoRecommendationService, NewVideoRecommendationService>();
        }
    }
}
