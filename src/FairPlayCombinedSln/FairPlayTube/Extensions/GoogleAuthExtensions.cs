using FairPlayCombined.Models.GoogleAuth;

namespace FairPlayTube.Extensions
{
    public static class GoogleAuthExtensions
    {
        public static string? GetConfigurationValue(this WebApplicationBuilder builder, string key)
        {
            var configurationValue = builder.Configuration[key] ?? 
                throw new InvalidOperationException($"{key} not found");
            return configurationValue;
        }
        public static GoogleAuthClientSecretInfo GetGoogleAuthClientSecretInfo(this WebApplicationBuilder builder)
        {
            var googleAuthClientId = builder.GetConfigurationValue("GoogleAuthClientId");

            var googleAuthProjectId = builder.GetConfigurationValue("GoogleAuthProjectId");

            var googleAuthUri = builder.GetConfigurationValue("GoogleAuthUri");

            var googleAuthTokenUri = builder.GetConfigurationValue("GoogleAuthTokenUri");

            var googleAuthProviderCertUri = builder.GetConfigurationValue("GoogleAuthProviderCertUri");

            var googleAuthClientSecret = builder.GetConfigurationValue("GoogleAuthClientSecret");

            var googleAuthRedirectUri = builder.GetConfigurationValue("GoogleAuthRedirectUri")!;

            GoogleAuthClientSecretInfo googleAuthClientSecretInfo = new()
            {
                installed = new Installed()
                {
                    auth_provider_x509_cert_url = googleAuthProviderCertUri,
                    auth_uri = googleAuthUri,
                    client_id = googleAuthClientId,
                    client_secret = googleAuthClientSecret,
                    project_id = googleAuthProjectId,
                    redirect_uris = [googleAuthRedirectUri],
                    token_uri = googleAuthTokenUri
                }
            };
            return googleAuthClientSecretInfo;
        }
    }
}
