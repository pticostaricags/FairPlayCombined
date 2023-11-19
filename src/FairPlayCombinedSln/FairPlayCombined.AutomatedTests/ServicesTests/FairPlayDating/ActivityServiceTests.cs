using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class ActivityServiceTests
    {
        public static readonly MsSqlContainer _msSqlContainer
        = new MsSqlBuilder().Build();
        [ClassInitialize]
#pragma warning disable IDE0060 // Remove unused parameter
        public static async Task ClassInitializeAsync(TestContext testContext)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            await _msSqlContainer.StartAsync();
        }

        [ClassCleanup()]
        public static async Task ClassCleanupAsync()
        {
            if (_msSqlContainer.State == DotNet.Testcontainers.Containers.TestcontainersStates.Running)
            {
                await _msSqlContainer.StopAsync();
            }
        }

        [TestMethod]
        public async Task Test_CreateActivityAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction => 
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<ActivityService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var activityService = sp.GetRequiredService<ActivityService>();
            CreateActivityModel createActivityModel = new CreateActivityModel()
            {
                Name = "TestModel"
            };
            await activityService.CreateActivityAsync(createActivityModel, CancellationToken.None);
            var result = await dbContext.Activity.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createActivityModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteActivityAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<ActivityService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var activityService = sp.GetRequiredService<ActivityService>();
            Activity entity = new Activity()
            {
                Name = "TestModel"
            };
            await dbContext.Activity.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.ActivityId);
            await activityService.DeleteActivityById(entity.ActivityId, CancellationToken.None);
            var itemsCount = await dbContext.Activity.CountAsync(CancellationToken.None);
            Assert.AreEqual(0,itemsCount);
        }
    }
}
