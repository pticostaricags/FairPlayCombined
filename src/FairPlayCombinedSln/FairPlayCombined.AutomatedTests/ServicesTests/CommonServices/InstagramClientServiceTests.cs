using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Services;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class InstagramClientServiceTests
    {
        [TestMethod]
        public async Task Test_GetUserInfoAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var instagramUserAccessToken = configuration["InstagramUserAccessToken"] ??
                throw new Exception("'InstagramUserAccessToken' is not in configuration");
            ServiceCollection services = new();
            services.AddTransient<ILogger<InstagramClientService>>(sp =>
            {
                var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
                var logger = loggerFactory!.CreateLogger<InstagramClientService>();
                return logger;
            });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
            services.AddTransient<IInstagramClientService,InstagramClientService>();
            services.AddHttpClient();
            var sp = services.BuildServiceProvider();
            var instagramClientService = sp.GetRequiredService<IInstagramClientService>();
            var result = await instagramClientService.GetUserInfoAsync("me", instagramUserAccessToken, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetUserMediaAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var instagramUserAccessToken = configuration["InstagramUserAccessToken"] ??
                throw new Exception("'InstagramUserAccessToken' is not in configuration");
            ServiceCollection services = new();
            services.AddTransient<ILogger<InstagramClientService>>(sp =>
            {
                var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
                var logger = loggerFactory!.CreateLogger<InstagramClientService>();
                return logger;
            });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
            services.AddTransient<IInstagramClientService, InstagramClientService>();
            services.AddHttpClient();
            var sp = services.BuildServiceProvider();
            var instagramClientService = sp.GetRequiredService<IInstagramClientService>();
            var result = await instagramClientService.GetUserMediaAsync("me", instagramUserAccessToken, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_CreateSingleMediaPostAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var instagramUserAccessToken = configuration["InstagramUserAccessToken"] ??
                throw new Exception("'InstagramUserAccessToken' is not in configuration");
            var instagramTestImageUrl = configuration["InstagramTestImageUrl"] ??
                throw new Exception("'InstagramTestImageUrl' is not in configuration");
            ServiceCollection services = new();
            services.AddTransient<ILogger<InstagramClientService>>(sp =>
            {
                var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
                var logger = loggerFactory!.CreateLogger<InstagramClientService>();
                return logger;
            });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
            services.AddTransient<IInstagramClientService, InstagramClientService>();
            services.AddHttpClient();
            var sp = services.BuildServiceProvider();
            var instagramClientService = sp.GetRequiredService<IInstagramClientService>();
            string username = "me";
            var result = await instagramClientService.CreateSingleMediaPostAsync(username, instagramUserAccessToken, instagramTestImageUrl, CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
