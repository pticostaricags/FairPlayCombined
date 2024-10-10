
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

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class LinkedInConnectionServiceTests : ServicesBase
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
        public async Task Test_ImportFromConnectionsFileAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var linkedInConnectionsFilePath = configuration["LinkedInConnectionsFilePath"] ??
                throw new Exception("'LinkedInConnectionsFilePath' is not in configuration");
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
            services.AddTransient<ILogger<LinkedInConnectionService>>(sp =>
            {
                var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
                var logger = loggerFactory!.CreateLogger<LinkedInConnectionService>();
                return logger;
            });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
            services.AddTransient<LinkedInConnectionService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            string testUserName = "fromuser@test.test";
            AspNetUsers testUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = testUserName,
                NormalizedUserName = testUserName.Normalize(),
                Email = testUserName,
                NormalizedEmail = testUserName.Normalize(),
            };
            await dbContext.AspNetUsers.AddAsync(testUser);
            await dbContext.SaveChangesAsync();
            TestUserProviderService.CurrentUserId = testUser.Id;
            var linkedInConnectionService = sp.GetRequiredService<LinkedInConnectionService>();
            var filePath = configuration["LinkedInConnectionsFilePath"];
            Stream fileStream = File.OpenRead(filePath!);
            await linkedInConnectionService.ImportFromConnectionsFileAsync(
                fileStream, CancellationToken.None);
            var connectionsCount = await dbContext.LinkedInConnection.CountAsync();
            Assert.IsTrue(connectionsCount > 0);
        }
    }
}
