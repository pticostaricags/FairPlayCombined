using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class TwitterClientServiceTests : ServicesBase
    {
        [TestMethod]
        public async Task Test_GetMyUserDataAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var twitterAccessToken = configuration["TwitterAccessToken"]!;
            ServiceCollection services = new();
            services.AddHttpClient();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<ILogger<LinkedInConnectionService>>(sp =>
            {
                var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
                var logger = loggerFactory!.CreateLogger<LinkedInConnectionService>();
                return logger;
            });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
            services.AddTransient<TwitterClientService>();
            var sp = services.BuildServiceProvider();
            TwitterClientService twitterClientService = sp.GetRequiredService<TwitterClientService>();
            var result = await twitterClientService
                .GetMyUserDataAsync(twitterAccessToken,CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
