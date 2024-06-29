using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayTube.ClientServices;
using FairPlayTube.ClientServices.CustomLocalization;
using FairPlayTube.ClientServices.KiotaClient;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
            builder.Services.AddMemoryCache();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IStringLocalizerFactory, ApiLocalizerFactory>();
            builder.Services.AddSingleton<IStringLocalizer, ApiLocalizer>();
            builder.Services.AddLocalization();
            builder.Services.AddScoped<LocalizationMessageHandler>();
            builder.Services.AddKeyedSingleton<ApiClient>("AnonymousApiClient",
                (sp, key) =>
                {
                    AnonymousAuthenticationProvider anonymousAuthenticationProvider = new();
                    HttpClientRequestAdapter httpClientRequestAdapter = new(anonymousAuthenticationProvider);
                    httpClientRequestAdapter.BaseUrl = apiBaseUrl;
                    ApiClient apiClient = new(httpClientRequestAdapter);
                    return apiClient;
                });

            builder.Services.AddTransient<IVideoInfoService, VideoInfoClientService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }

    public static class AdditionalSetup
    {
        internal static readonly Assembly[] AdditionalAssemblies =
                [typeof(FairPlayTube.SharedUI.Components.Pages.Home).Assembly];
    }
}