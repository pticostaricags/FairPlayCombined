using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayDating.LikedUserProfile;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class LikedUserProfileServiceTests
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
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleLikedUserProfile in dbContext.LikedUserProfile)
            {
                dbContext.LikedUserProfile.Remove(singleLikedUserProfile);
            }
            foreach (var singleUser in dbContext.AspNetUsers)
            {
                dbContext.Remove(singleUser);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateLikedUserProfileAsync()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDbContext(services);
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var LikedUserProfileService = sp.GetRequiredService<LikedUserProfileService>();
            CreateLikedUserProfileModel createLikedUserProfileModel = new CreateLikedUserProfileModel()
            {
                LikedApplicationUserId = toUser.Id,
                LikingApplicationUserId = fromUser.Id
            };
            await LikedUserProfileService.CreateLikedUserProfileAsync(createLikedUserProfileModel, CancellationToken.None);
            var result = await dbContext.LikedUserProfile.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createLikedUserProfileModel.LikingApplicationUserId, result.LikingApplicationUserId);
        }

        private static void RegisterDbContext(ServiceCollection services)
        {
            var cs = _msSqlContainer.GetConnectionString();
            Extensions.EnhanceConnectionString(nameof(FairPlayCombined.AutomatedTests), ref cs);
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.AddInterceptors(
                        new SaveChangesInterceptor(new TestUserProviderService())
                        );
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
        }

        private static async Task<(AspNetUsers fromUser, AspNetUsers toUser)> CreateTestRecordsAsync(FairPlayCombinedDbContext dbContext)
        {
            string fromUserName = "fromuser@test.test";
            string toUserName = "toUser@test.test";
            AspNetUsers fromUser = new AspNetUsers()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = fromUserName,
                NormalizedUserName = fromUserName.Normalize(),
                Email = fromUserName,
                NormalizedEmail = fromUserName.Normalize()
            };
            AspNetUsers toUser = new AspNetUsers()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = toUserName,
                NormalizedUserName = toUserName.Normalize(),
                Email = toUserName,
                NormalizedEmail = toUserName.Normalize()
            };
            await dbContext.AspNetUsers.AddRangeAsync(fromUser);
            await dbContext.AspNetUsers.AddRangeAsync(toUser);
            await dbContext.SaveChangesAsync();
            return (fromUser, toUser);
        }

        [TestMethod]
        public async Task Test_DeleteLikedUserProfileAsync()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDbContext(services);
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var LikedUserProfileService = sp.GetRequiredService<LikedUserProfileService>();
            LikedUserProfile entity = new LikedUserProfile()
            {
                LikedApplicationUserId = toUser.Id,
                LikingApplicationUserId = fromUser.Id,
            };
            await dbContext.LikedUserProfile.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.LikedUserProfileId);
            await LikedUserProfileService.DeleteLikedUserProfileByIdAsync(entity.LikedUserProfileId, CancellationToken.None);
            var itemsCount = await dbContext.LikedUserProfile.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedLikedUserProfileAsync()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDbContext(services);
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var LikedUserProfileService = sp.GetRequiredService<LikedUserProfileService>();
            LikedUserProfile entity = new LikedUserProfile()
            {
                LikedApplicationUserId = toUser.Id,
                LikingApplicationUserId = fromUser.Id,
            };
            await dbContext.LikedUserProfile.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.LikedUserProfileId);
            var result = await LikedUserProfileService.GetPaginatedLikedUserProfileAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new SortingItem()
                        {
                            PropertyName = nameof(LikedUserProfileModel.LikedApplicationUserId),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].LikedUserProfileId, entity.LikedUserProfileId);
        }

        [TestMethod]
        public async Task Test_GetLikedUserProfileByIdAsync()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDbContext(services);
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var LikedUserProfileService = sp.GetRequiredService<LikedUserProfileService>();
            LikedUserProfile entity = new LikedUserProfile()
            {
                LikedApplicationUserId = toUser.Id,
                LikingApplicationUserId = fromUser.Id,
            };
            await dbContext.LikedUserProfile.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.LikedUserProfileId);
            var result = await LikedUserProfileService.GetLikedUserProfileByIdAsync(entity.LikedUserProfileId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.LikingApplicationUserId, result.LikingApplicationUserId);
        }
    }
}
