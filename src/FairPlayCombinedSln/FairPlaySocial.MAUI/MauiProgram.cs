using FairPlayCombined.Common;
using FairPlaySocial.ClientServices;
using FairPlaySocial.MAUI.Auth;
using FairPlaySocial.MAUI.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Reflection;

namespace FairPlaySocial.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            string apiBaseUrl = String.Empty;
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
            apiBaseUrl = Constants.MAUI.DefaultApiBindings.FairPlaySocial;
#else
            apiBaseUrl = "[CHANGE TO YOUR URL]";
#endif
            ApiInfo.ApiBaseUrl = apiBaseUrl;
            builder.Services.AddSingleton<IAccessTokenProvider, CustomAccessTokenAuthenticationProvider>();
            builder.Services.AddKeyedSingleton<ApiClient>("AnonymousApiClient",
                (sp, key) =>
                {
                    AnonymousAuthenticationProvider anonymousAuthenticationProvider=
                    new AnonymousAuthenticationProvider();
                    HttpClientRequestAdapter httpClientRequestAdapter = new(anonymousAuthenticationProvider);
                    httpClientRequestAdapter.BaseUrl = apiBaseUrl;
                    ApiClient apiClient = new(httpClientRequestAdapter);
                    return apiClient;
                });
            builder.Services.AddKeyedSingleton<ApiClient>("AuthenticatedApiClient",
                (sp, key) => 
                {
                    var accesstokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
                    BaseBearerTokenAuthenticationProvider baseBearerTokenAuthenticationProvider =
            new BaseBearerTokenAuthenticationProvider(accesstokenProvider);
                    HttpClientRequestAdapter httpClientRequestAdapter = new(baseBearerTokenAuthenticationProvider);
                    httpClientRequestAdapter.BaseUrl = apiBaseUrl;
                    ApiClient apiClient = new(httpClientRequestAdapter);
                    return apiClient;
                });
            return builder.Build();
        }
        public class CustomAccessTokenAuthenticationProvider : IAccessTokenProvider
        {
            public AllowedHostsValidator AllowedHostsValidator => new AllowedHostsValidator();

            public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(UserContext.AccessToken ?? String.Empty);
            }
        }
    }
}