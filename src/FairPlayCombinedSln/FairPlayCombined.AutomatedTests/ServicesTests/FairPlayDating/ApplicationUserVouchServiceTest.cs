using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayDating.ApplicationUserVouch;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class ApplicationUserVouchServiceTests : ServicesBase
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
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<ApplicationUserVouchService>();
            services.AddTransient<ILogger<ApplicationUserVouchService>>(p =>
            {
                return logger;
            });
            services.AddTransient<ApplicationUserVouchService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleApplicationUserVouch in dbContext.ApplicationUserVouch)
            {
                dbContext.ApplicationUserVouch.Remove(singleApplicationUserVouch);
            }
            foreach (var singleUser in dbContext.AspNetUsers)
            {
                dbContext.Remove(singleUser);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateApplicationUserVouchAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<ApplicationUserVouchService>();
            services.AddTransient<ILogger<ApplicationUserVouchService>>(p =>
            {
                return logger;
            });
            services.AddTransient<ApplicationUserVouchService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var ApplicationUserVouchService = sp.GetRequiredService<ApplicationUserVouchService>();
            CreateApplicationUserVouchModel createApplicationUserVouchModel = new()
            {
                Description = "AT DESC",
                FromApplicationUserId = fromUser.Id,
                ToApplicationUserId = toUser.Id
            };
            await ApplicationUserVouchService.CreateApplicationUserVouchAsync(createApplicationUserVouchModel, CancellationToken.None);
            var result = await dbContext.ApplicationUserVouch.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createApplicationUserVouchModel.ToApplicationUserId, result.ToApplicationUserId);
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
            AspNetUsers fromUser = await CreateFromUserAsync(dbContext);
            AspNetUsers toUser = await CreateToUserAsync(dbContext);
            return (fromUser, toUser);
        }

        [TestMethod]
        public async Task Test_DeleteApplicationUserVouchAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<ApplicationUserVouchService>();
            services.AddTransient<ILogger<ApplicationUserVouchService>>(p =>
            {
                return logger;
            });
            services.AddTransient<ApplicationUserVouchService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var ApplicationUserVouchService = sp.GetRequiredService<ApplicationUserVouchService>();
            ApplicationUserVouch entity = new()
            {
                Description = "AT DESC",
                FromApplicationUserId = fromUser.Id,
                ToApplicationUserId = toUser.Id
            };
            await dbContext.ApplicationUserVouch.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.ApplicationUserVouchId);
            await ApplicationUserVouchService.DeleteApplicationUserVouchByIdAsync(entity.ApplicationUserVouchId, CancellationToken.None);
            var itemsCount = await dbContext.ApplicationUserVouch.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedApplicationUserVouchAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<ApplicationUserVouchService>();
            services.AddTransient<ILogger<ApplicationUserVouchService>>(p =>
            {
                return logger;
            });
            services.AddTransient<ApplicationUserVouchService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var ApplicationUserVouchService = sp.GetRequiredService<ApplicationUserVouchService>();
            ApplicationUserVouch entity = new()
            {
                Description = "AT DESC",
                FromApplicationUserId = fromUser.Id,
                ToApplicationUserId = toUser.Id
            };
            await dbContext.ApplicationUserVouch.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.ApplicationUserVouchId);
            var result = await ApplicationUserVouchService.GetPaginatedApplicationUserVouchAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems =
                    [
                        new SortingItem()
                        {
                            PropertyName = nameof(ApplicationUserVouchModel.Description),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    ]
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].ApplicationUserVouchId, entity.ApplicationUserVouchId);
        }

        [TestMethod]
        public async Task Test_GetApplicationUserVouchByIdAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<ApplicationUserVouchService>();
            services.AddTransient<ILogger<ApplicationUserVouchService>>(p =>
            {
                return logger;
            });
            services.AddTransient<ApplicationUserVouchService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, AspNetUsers toUser) = await CreateTestRecordsAsync(dbContext);
            var ApplicationUserVouchService = sp.GetRequiredService<ApplicationUserVouchService>();
            ApplicationUserVouch entity = new()
            {
                Description = "AT DESC",
                FromApplicationUserId = fromUser.Id,
                ToApplicationUserId = toUser.Id
            };
            await dbContext.ApplicationUserVouch.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.ApplicationUserVouchId);
            var result = await ApplicationUserVouchService.GetApplicationUserVouchByIdAsync(entity.ApplicationUserVouchId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Description, result.Description);
        }
    }
}
