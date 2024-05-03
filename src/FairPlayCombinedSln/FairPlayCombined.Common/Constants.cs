namespace FairPlayCombined.Common
{
    public static class Constants
    {
        public static class Routes
        {
            public static class SignalRHubs
            {
                public const string UserMessageHub = $"/{nameof(UserMessageHub)}";
            }
            public static class FairPlayTubeRoutes
            {
                public static class CreatorRoutes
                {
                    public const string MyVideos = $"/Creator/{nameof(MyVideos)}";
                    public const string MyVideoViewers = $"/Creator/{nameof(MyVideoViewers)}";
                }
            }
        }
        public static class ConfigurationSecretsKeys
        {
            public const string OPENAI_KEY = "OpenAIKey";
            public const string GENERATE_DALL3_IMAGE_URL_KEY = "GenerateDall3ImageUrl";
            public const string OPENAI_CHAT_COMPLETION_URL_KEY = "OpenAIChatCompletionsUrl";
            public const string GOOGLE_AUTH_CLIENT_ID_KEY = "GoogleAuthClientId";
            public const string AZURE_OPENAI_ENDPOINT_KEY = "AzureOpenAIEndpoint";
            public const string AZURE_OPENAI_KEY_KEY = "AzureOpenAIKey";
            public const string AZURE_OPENAI_DEPLOYMENT_NAME_KEY = "AzureOpenAIDeploymentName";
            public const string AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY = "AzureVideoIndexerAccountId";
            public const string AZURE_VIDEOINDEXER_LOCATION_KEY = "AzureVideoIndexerLocation";
            public const string AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY = "AzureVideoIndexerResourceGroup";
            public const string AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY = "AzureVideoIndexerResourceName";
            public const string AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY = "AzureVideoIndexerSubscriptionId";
            public const string GOOGLE_GEMINI_KEY_KEY = "GoogleGeminiKey";
            public const string AZURE_CONTENT_SAFETY_ENDPOINT_KEY = "AzureContentSafetyEndpoint";
            public const string AZURE_CONTENT_SAFETY_KEY_KEY = "AzureContentSafetyKey";
        }
        public static class GeoCoordinates
        {
            /// <summary>
            /// 4326 refers to WGS 84, a standard used in GPS and other geographic systems.
            /// Check: https://learn.microsoft.com/en-us/ef/core/modeling/spatial
            /// </summary>
            public const int SRID = 4326;
        }
        public static class Matches
        {
            public const int MaxAllowedAgeDifference = 10;
        }
        public static class RoleName
        {
            public const string SystemAdmin = nameof(SystemAdmin);
            public const string User = nameof(User);
        }
        public static class Pagination
        {
            public const int PageSize = 10;
        }
        public static class CacheConfiguration
        {
            public static readonly TimeSpan LocalizationCacheDuration = TimeSpan.FromSeconds(5);
        }
    }
}
