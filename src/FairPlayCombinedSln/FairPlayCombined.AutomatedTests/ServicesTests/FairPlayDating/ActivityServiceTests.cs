﻿using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating;
using FairPlayCombined.Models.Pagination;
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

        [TestCleanup]
        public async Task TestCleanupAsync()
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
            foreach (var singleActivity in dbContext.Activity)
            {
                dbContext.Activity.Remove(singleActivity);
            }
            await dbContext.SaveChangesAsync();
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
            await activityService.DeleteActivityByIdAsync(entity.ActivityId, CancellationToken.None);
            var itemsCount = await dbContext.Activity.CountAsync(CancellationToken.None);
            Assert.AreEqual(0,itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedActivityAsync()
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
            var result = await activityService.GetPaginatedActivityAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems=new SortingItem[]
                    {
                        new SortingItem()
                        {
                            PropertyName = nameof(ActivityModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].ActivityId, entity.ActivityId);
        }

        [TestMethod]
        public async Task Test_GetActivityByIdAsync()
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
            var result = await activityService.GetActivityByIdAsync(entity.ActivityId, CancellationToken.None);
            Assert.IsNotNull (result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}