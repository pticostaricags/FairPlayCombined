using FairPlayCombined.Models.GoogleAuth;

namespace FairPlayTube.Extensions
{
    public static class GoogleAuthExtensions
    {
        public static GoogleAuthClientSecretInfo GetGoogleAuthClientSecretInfo(this WebApplicationBuilder builder)
        {
            var googleAuthClientId = builder.Configuration["GoogleAuthClientId"] ??
        throw new InvalidOperationException("'GoogleAuthClientId' not found");

            var googleAuthProjectId = builder.Configuration["GoogleAuthProjectId"] ??
                    throw new InvalidOperationException("'GoogleAuthProjectId' not found");

            var googleAuthUri = builder.Configuration["GoogleAuthUri"] ??
                    throw new InvalidOperationException("'GoogleAuthUri' not found");

            var googleAuthTokenUri = builder.Configuration["GoogleAuthTokenUri"] ??
                    throw new InvalidOperationException("'GoogleAuthTokenUri' not found");

            var googleAuthProviderCertUri = builder.Configuration["GoogleAuthProviderCertUri"] ??
                    throw new InvalidOperationException("'GoogleAuthProviderCertUri' not found");

            var googleAuthClientSecret = builder.Configuration["GoogleAuthClientSecret"] ??
                    throw new InvalidOperationException("'GoogleAuthClientSecret' not found");

            var googleAuthRedirectUri = builder.Configuration["GoogleAuthRedirectUri"] ??
                    throw new InvalidOperationException("'GoogleAuthRedirectUri' not found");

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
