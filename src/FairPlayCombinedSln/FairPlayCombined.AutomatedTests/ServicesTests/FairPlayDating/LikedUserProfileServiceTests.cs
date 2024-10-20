using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayDating.LikedUserProfile;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class LikedUserProfileServiceTests : ServicesBase
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
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<LikedUserProfileService>();
            services.AddTransient<ILogger<LikedUserProfileService>>(p =>
            {
                return logger;
            });
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var LikedUserProfileService = sp.GetRequiredService<LikedUserProfileService>();
            CreateLikedUserProfileModel createLikedUserProfileModel = new()
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

        private static async Task<(AspNetUsers fromUser, AspNetUsers toUser)> CreateTestRecordsAsync(FairPlayCombinedDbContext dbContext)
        {
            string fromUserName = "fromuser@test.test";
            string toUserName = "toUser@test.test";
            AspNetUsers fromUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = fromUserName,
                NormalizedUserName = fromUserName.Normalize(),
                Email = fromUserName,
                NormalizedEmail = fromUserName.Normalize(),
                Name = "AT FROM NAME",
                Lastname = "AT FROM LASTNAME"
            };
            AspNetUsers toUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = toUserName,
                NormalizedUserName = toUserName.Normalize(),
                Email = toUserName,
                NormalizedEmail = toUserName.Normalize(),
                Name = "AT TO NAME",
                Lastname = "AT TO LASTNAME"
            };
            await dbContext.AspNetUsers.AddRangeAsync(fromUser);
            await dbContext.AspNetUsers.AddRangeAsync(toUser);
            await dbContext.SaveChangesAsync();
            return (fromUser, toUser);
        }

        [TestMethod]
        public async Task Test_DeleteLikedUserProfileAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<LikedUserProfileService>();
            services.AddTransient<ILogger<LikedUserProfileService>>(p =>
            {
                return logger;
            });
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var LikedUserProfileService = sp.GetRequiredService<LikedUserProfileService>();
            LikedUserProfile entity = new()
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
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<LikedUserProfileService>();
            services.AddTransient<ILogger<LikedUserProfileService>>(p =>
            {
                return logger;
            });
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var LikedUserProfileService = sp.GetRequiredService<LikedUserProfileService>();
            LikedUserProfile entity = new()
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
                    SortingItems =
                    [
                        new SortingItem()
                        {
                            PropertyName = nameof(LikedUserProfileModel.LikedApplicationUserId),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    ]
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].LikedUserProfileId, entity.LikedUserProfileId);
        }

        [TestMethod]
        public async Task Test_GetLikedUserProfileByIdAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<LikedUserProfileService>();
            services.AddTransient<ILogger<LikedUserProfileService>>(p =>
            {
                return logger;
            });
            services.AddTransient<LikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var LikedUserProfileService = sp.GetRequiredService<LikedUserProfileService>();
            LikedUserProfile entity = new()
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
