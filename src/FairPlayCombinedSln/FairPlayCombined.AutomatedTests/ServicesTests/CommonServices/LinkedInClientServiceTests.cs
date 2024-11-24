using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
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

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class LinkedInClientServiceTests : ServicesBase
    {
        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleLinkedInConnection in dbContext.LinkedInConnection)
            {
                dbContext.LinkedInConnection.Remove(singleLinkedInConnection);
            }
            foreach (var singleUser in dbContext.AspNetUsers)
            {
                dbContext.AspNetUsers.Remove(singleUser);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateImageShareAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var linkedInUserAccessToken = configuration["LinkedInUserAccessToken"] ??
                throw new Exception("'LinkedInUserAccessToken' is not in configuration");
            var linkedInTestImagePath = configuration["LinkedInTestImagePath"] ??
                throw new Exception("'LinkedInTestImagePath' is not in configuration");
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.AddInterceptors(
                        new SaveChangesInterceptor(new TestUserProviderService())
                        );
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<ILogger<LinkedInClientService>>(sp =>
            {
                var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
                var logger = loggerFactory!.CreateLogger<LinkedInClientService>();
                return logger;
            });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
            services.AddTransient<LinkedInClientService>();
            services.AddHttpClient();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            AspNetUsers testUser = await CreateFromUserAsync(dbContext);
            TestUserProviderService.CurrentUserId = testUser.Id;
            await dbContext.AspNetUserTokens.AddAsync(new AspNetUserTokens()
            {
                LoginProvider = "LinkedIn",
                Name = "access_token",
                UserId = testUser.Id,
                Value = linkedInUserAccessToken
            });
            await dbContext.SaveChangesAsync();
            using var stream = File.Open(linkedInTestImagePath, FileMode.Open);
            var linkedInClientService = sp.GetRequiredService<LinkedInClientService>();
            var result = 
            await linkedInClientService
                .CreateImageShareAsync("Test image", stream,
                Path.GetFileNameWithoutExtension(linkedInTestImagePath),
                "Media description",
                "Media Title", CancellationToken.None);
            Assert.IsTrue(result);
        }
    }
}
