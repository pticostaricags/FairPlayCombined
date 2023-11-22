using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayDating.NotLikedUserProfile;
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
    public class NotLikedUserProfileServiceTests : ServicesBase
    {

        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<NotLikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleNotLikedUserProfile in dbContext.NotLikedUserProfile)
            {
                dbContext.NotLikedUserProfile.Remove(singleNotLikedUserProfile);
            }
            foreach (var singleUser in dbContext.AspNetUsers)
            {
                dbContext.Remove(singleUser);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateNotLikedUserProfileAsync()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDbContext(services);
            services.AddTransient<NotLikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var NotLikedUserProfileService = sp.GetRequiredService<NotLikedUserProfileService>();
            CreateNotLikedUserProfileModel createNotLikedUserProfileModel = new CreateNotLikedUserProfileModel()
            {
                NotLikedApplicationUserId = toUser.Id,
                NotLikingApplicationUserId = fromUser.Id
            };
            await NotLikedUserProfileService.CreateNotLikedUserProfileAsync(createNotLikedUserProfileModel, CancellationToken.None);
            var result = await dbContext.NotLikedUserProfile.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createNotLikedUserProfileModel.NotLikingApplicationUserId, result.NotLikingApplicationUserId);
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
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
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
        public async Task Test_DeleteNotLikedUserProfileAsync()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDbContext(services);
            services.AddTransient<NotLikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var NotLikedUserProfileService = sp.GetRequiredService<NotLikedUserProfileService>();
            NotLikedUserProfile entity = new NotLikedUserProfile()
            {
                NotLikedApplicationUserId = toUser.Id,
                NotLikingApplicationUserId = fromUser.Id,
            };
            await dbContext.NotLikedUserProfile.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.NotLikedUserProfileId);
            await NotLikedUserProfileService.DeleteNotLikedUserProfileByIdAsync(entity.NotLikedUserProfileId, CancellationToken.None);
            var itemsCount = await dbContext.NotLikedUserProfile.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedNotLikedUserProfileAsync()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDbContext(services);
            services.AddTransient<NotLikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var NotLikedUserProfileService = sp.GetRequiredService<NotLikedUserProfileService>();
            NotLikedUserProfile entity = new NotLikedUserProfile()
            {
                NotLikedApplicationUserId = toUser.Id,
                NotLikingApplicationUserId = fromUser.Id,
            };
            await dbContext.NotLikedUserProfile.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.NotLikedUserProfileId);
            var result = await NotLikedUserProfileService.GetPaginatedNotLikedUserProfileAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new SortingItem()
                        {
                            PropertyName = nameof(NotLikedUserProfileModel.NotLikedApplicationUserId),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].NotLikedUserProfileId, entity.NotLikedUserProfileId);
        }

        [TestMethod]
        public async Task Test_GetNotLikedUserProfileByIdAsync()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDbContext(services);
            services.AddTransient<NotLikedUserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var NotLikedUserProfileService = sp.GetRequiredService<NotLikedUserProfileService>();
            NotLikedUserProfile entity = new NotLikedUserProfile()
            {
                NotLikedApplicationUserId = toUser.Id,
                NotLikingApplicationUserId = fromUser.Id,
            };
            await dbContext.NotLikedUserProfile.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.NotLikedUserProfileId);
            var result = await NotLikedUserProfileService.GetNotLikedUserProfileByIdAsync(entity.NotLikedUserProfileId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.NotLikingApplicationUserId, result.NotLikingApplicationUserId);
        }
    }
}
