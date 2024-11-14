using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayTube.ClientServices;
using FairPlayTube.ClientServices.CustomLocalization;
using FairPlayTube.ClientServices.KiotaClient;
using FairPlayTube.MAUI.Authentication;
using FairPlayTube.MAUI.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Reflection;

namespace FairPlayTube.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var apiBaseUrl = string.Empty;
            var builder = MauiApp.CreateBuilder();
#if DEBUG && WINDOWS
            apiBaseUrl = "https://localhost:7021";
#endif
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddFluentUIComponents();
            builder.Services.AddMemoryCache();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<ComponentStatePersistenceManager>();
            builder.Services.AddSingleton<PersistentComponentState>(sp =>
            {
                var manager = sp.GetRequiredService<ComponentStatePersistenceManager>();
                return manager.State;
            });
            builder.Services.AddSingleton<IStringLocalizerFactory, ApiLocalizerFactory>();
            builder.Services.AddSingleton<IStringLocalizer, ApiLocalizer>();
            builder.Services.AddLocalization();
            builder.Services.AddOptions();
            builder.Services.TryAddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddHttpClient("DefaultHttpClient", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler<LocalizationMessageHandler>();
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DefaultHttpClient"));
            builder.Services.AddScoped<LocalizationMessageHandler>();
            builder.Services.AddSingleton<IAccessTokenProvider, CustomAccessTokenAuthenticationProvider>();
            builder.Services.AddKeyedSingleton<ApiClient>("AnonymousApiClient",
                (sp, key) =>
                {
                    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                    var httpClient = httpClientFactory.CreateClient("DefaultHttpClient");
                    AnonymousAuthenticationProvider anonymousAuthenticationProvider = new();
                    HttpClientRequestAdapter httpClientRequestAdapter = new(anonymousAuthenticationProvider,
                        httpClient: httpClient);
                    httpClientRequestAdapter.BaseUrl = apiBaseUrl;
                    ApiClient apiClient = new(httpClientRequestAdapter);
                    return apiClient;
                });
            builder.Services.AddKeyedSingleton<ApiClient>("AuthenticatedApiClient",
                (sp, key) =>
                {
                    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                    var httpClient = httpClientFactory.CreateClient("DefaultHttpClient");
                    var accesstokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
                    BaseBearerTokenAuthenticationProvider baseBearerTokenAuthenticationProvider =
                    new(accesstokenProvider);
                    HttpClientRequestAdapter httpClientRequestAdapter = 
                    new(baseBearerTokenAuthenticationProvider, 
                    httpClient: httpClient);
                    httpClientRequestAdapter.BaseUrl = apiBaseUrl;
                    ApiClient apiClient = new(httpClientRequestAdapter);
                    return apiClient;
                });
            builder.Services.AddSingleton<IApiResolver, ApiResolver>(sp=>new ApiResolver(apiBaseUrl));
            builder.Services.AddTransient<IVideoInfoService, VideoInfoClientService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }

    public class CustomAccessTokenAuthenticationProvider : IAccessTokenProvider
    {
        public AllowedHostsValidator AllowedHostsValidator => new();

        public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
        {
            var result = UserContext.AccessToken ?? String.Empty;
            return Task.FromResult(result);
        }
    }

    public static class AdditionalSetup
    {
        internal static readonly Assembly[] AdditionalAssemblies =
                [typeof(FairPlayTube.SharedUI.Components.Pages.Home).Assembly];
    }
}