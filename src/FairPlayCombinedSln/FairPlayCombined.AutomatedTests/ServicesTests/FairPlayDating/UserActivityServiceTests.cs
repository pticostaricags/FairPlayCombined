using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayDating.UserActivity;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class UserActivityServiceTests : ServicesBase
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
            services.AddTransient<UserActivityService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleUserActivity in dbContext.UserActivity)
            {
                dbContext.UserActivity.Remove(singleUserActivity);
            }
            foreach (var singleFrequency in dbContext.Frequency)
            {
                dbContext.Frequency.Remove(singleFrequency);
            }
            foreach (var singleActivity in dbContext.Activity)
            {
                dbContext.Activity.Remove(singleActivity);
            }
            foreach (var singleUser in dbContext.AspNetUsers)
            {
                dbContext.Remove(singleUser);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateUserActivityAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<UserActivityService>();
            services.AddTransient<ILogger<UserActivityService>>(p =>
            {
                return logger;
            });
            services.AddTransient<UserActivityService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, _, Activity activity) = await CreateTestRecordsAsync(dbContext);
            var UserActivityService = sp.GetRequiredService<UserActivityService>();
            Frequency frequency = new()
            {
                Name = "TEST FREQUENCY",
            };
            await dbContext.Frequency.AddAsync(frequency);
            await dbContext.SaveChangesAsync();
            CreateUserActivityModel createUserActivityModel = new()
            {
                FrequencyId = frequency.FrequencyId,
                ApplicationUserId = fromUser.Id,
                ActivityId = activity.ActivityId
            };
            await UserActivityService.CreateUserActivityAsync(createUserActivityModel, CancellationToken.None);
            var result = await dbContext.UserActivity.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createUserActivityModel.ApplicationUserId, result.ApplicationUserId);
        }

        private static void RegisterDbContext(ServiceCollection services)
        {
            var cs = _msSqlContainer!.GetConnectionString();
            Extensions.EnhanceConnectionString(nameof(FairPlayCombined.AutomatedTests), ref cs);
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.AddInterceptors(
                        new SaveChangesInterceptor(new TestUserProviderService())
                        );
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
        }

        private static async Task<(AspNetUsers fromUser, AspNetUsers toUser, Activity activity)>
            CreateTestRecordsAsync(FairPlayCombinedDbContext dbContext)
        {
            string fromUserName = "fromuser@test.test";
            string toUserName = "toUser@test.test";
            AspNetUsers fromUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = fromUserName,
                NormalizedUserName = fromUserName.Normalize(),
                Email = fromUserName,
                NormalizedEmail = fromUserName.Normalize()
            };
            AspNetUsers toUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = toUserName,
                NormalizedUserName = toUserName.Normalize(),
                Email = toUserName,
                NormalizedEmail = toUserName.Normalize()
            };
            await dbContext.AspNetUsers.AddRangeAsync(fromUser);
            await dbContext.AspNetUsers.AddRangeAsync(toUser);
            Activity activity = new()
            {
                Name = "TEST ACTIVITY"
            };
            await dbContext.Activity.AddAsync(activity);
            await dbContext.SaveChangesAsync();
            return (fromUser, toUser, activity);
        }

        [TestMethod]
        public async Task Test_DeleteUserActivityAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<UserActivityService>();
            services.AddTransient<ILogger<UserActivityService>>(p =>
            {
                return logger;
            });
            services.AddTransient<UserActivityService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, _, Activity activity) = await CreateTestRecordsAsync(dbContext);
            var UserActivityService = sp.GetRequiredService<UserActivityService>();
            Frequency frequency = new()
            {
                Name = "TEST FREQUENCY",
            };
            await dbContext.Frequency.AddAsync(frequency);
            await dbContext.SaveChangesAsync();
            UserActivity entity = new()
            {
                ApplicationUserId = fromUser.Id,
                FrequencyId = frequency.FrequencyId,
                ActivityId = activity.ActivityId
            };
            await dbContext.UserActivity.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.UserActivityId);
            await UserActivityService.DeleteUserActivityByIdAsync(entity.UserActivityId, CancellationToken.None);
            var itemsCount = await dbContext.UserActivity.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedUserActivityAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<UserActivityService>();
            services.AddTransient<ILogger<UserActivityService>>(p =>
            {
                return logger;
            });
            services.AddTransient<UserActivityService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, _, Activity activity) = await CreateTestRecordsAsync(dbContext);
            var UserActivityService = sp.GetRequiredService<UserActivityService>();
            Frequency frequency = new()
            {
                Name = "TEST FREQUENCY",
            };
            await dbContext.Frequency.AddAsync(frequency);
            await dbContext.SaveChangesAsync();
            UserActivity entity = new()
            {
                ApplicationUserId = fromUser.Id,
                FrequencyId = frequency.FrequencyId,
                ActivityId = activity.ActivityId
            };
            await dbContext.UserActivity.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.UserActivityId);
            var result = await UserActivityService.GetPaginatedUserActivityAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new()
                        {
                            PropertyName = nameof(UserActivityModel.UserActivityId),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].UserActivityId, entity.UserActivityId);
        }

        [TestMethod]
        public async Task Test_GetUserActivityByIdAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<UserActivityService>();
            services.AddTransient<ILogger<UserActivityService>>(p =>
            {
                return logger;
            });
            services.AddTransient<UserActivityService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, _, Activity activity) = await CreateTestRecordsAsync(dbContext);
            var UserActivityService = sp.GetRequiredService<UserActivityService>();
            Frequency frequency = new()
            {
                Name = "TEST FREQUENCY",
            };
            await dbContext.Frequency.AddAsync(frequency);
            await dbContext.SaveChangesAsync();
            UserActivity entity = new()
            {
                ApplicationUserId = fromUser.Id,
                FrequencyId = frequency.FrequencyId,
                ActivityId = activity.ActivityId
            };
            await dbContext.UserActivity.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.UserActivityId);
            var result = await UserActivityService.GetUserActivityByIdAsync(entity.UserActivityId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.UserActivityId, result.UserActivityId);
        }
    }
}
