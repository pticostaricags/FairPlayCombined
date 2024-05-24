
using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class PromptGeneratorServiceTests : ServicesBase
    {
        [TestMethod]
        public async Task Test_PromptGeneratorService()
        {
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
            services.AddTransient<ILogger<PromptGeneratorService>>(sp =>
            {
                var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
                var logger = loggerFactory!.CreateLogger<PromptGeneratorService>();
                return logger;
            });
            services.AddTransient<PromptGeneratorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            string promptName = "YouTubeThumbnail";
            string promptBaseText = "YouTube Thumbnail for video based on the information I'll provide, " +
                "include a photo-realistic image of a person pointing upwards, " +
                "the person must at the bottom right corner of the image.";
            await dbContext.Prompt.AddAsync(new DataAccess.Models.dboSchema.Prompt()
            {
                BaseText = promptBaseText,
                PromptName = promptName,
                PromptVariable =
                [
                    new()
                    {
                        VariableName = "Video Title",
                    },
                    new()
                    {
                        VariableName="Video Description"
                    }
                ]
            }, cancellationToken: CancellationToken.None);
            await dbContext.SaveChangesAsync();
            var promptGeneratorService = sp.GetRequiredService<PromptGeneratorService>();
            var result = await promptGeneratorService.GetPromptCompleteInfoAsync(
                promptName, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(expected: promptName, result.PromptName);
        }
    }
}
