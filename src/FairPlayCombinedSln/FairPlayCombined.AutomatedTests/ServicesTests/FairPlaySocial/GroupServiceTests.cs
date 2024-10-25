using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.FairPlaySocial.Group;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlaySocial;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class GroupServiceTests : ServicesBase
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
            services.AddTransient<GroupService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleGroup in dbContext.Group)
            {
                dbContext.Group.Remove(singleGroup);
            }
            foreach (var singleUser in dbContext.AspNetUsers)
            {
                dbContext.AspNetUsers.Remove(singleUser);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateGroupAsync()
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
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<GroupService>();
            services.AddTransient<ILogger<GroupService>>(p =>
            {
                return logger;
            });
            services.AddTransient<GroupService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            AspNetUsers testUser = await CreateFromUserAsync(dbContext);
            var GroupService = sp.GetRequiredService<GroupService>();
            CreateGroupModel createGroupModel = new()
            {
                Name = "TestModel",
                Description = "Description",
                OwnerApplicationUserId = testUser.Id,
                TopicTag = "AT Tag"
            };
            await GroupService.CreateGroupAsync(createGroupModel, CancellationToken.None);
            var result = await dbContext.Group.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createGroupModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteGroupAsync()
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
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<GroupService>();
            services.AddTransient<ILogger<GroupService>>(p =>
            {
                return logger;
            });
            services.AddTransient<GroupService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var GroupService = sp.GetRequiredService<GroupService>();
            AspNetUsers testUser = await CreateFromUserAsync(dbContext);
            DataAccess.Models.FairPlaySocialSchema.Group entity = new()
            {
                Name = "TestModel",
                Description = "Description",
                OwnerApplicationUserId = testUser.Id,
                TopicTag = "AT Tag"
            };
            await dbContext.Group.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.GroupId);
            await GroupService.DeleteGroupByIdAsync(entity.GroupId, CancellationToken.None);
            var itemsCount = await dbContext.Group.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedGroupAsync()
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
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<GroupService>();
            services.AddTransient<ILogger<GroupService>>(p =>
            {
                return logger;
            });
            services.AddTransient<GroupService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var GroupService = sp.GetRequiredService<GroupService>();
            AspNetUsers testUser = await CreateToUserAsync(dbContext);
            DataAccess.Models.FairPlaySocialSchema.Group entity = new()
            {
                Name = "TestModel",
                Description = "Description",
                OwnerApplicationUserId = testUser.Id,
                TopicTag = "AT Tag"
            };
            await dbContext.Group.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.GroupId);
            var result = await GroupService.GetPaginatedGroupAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new()
                        {
                            PropertyName = nameof(GroupModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].GroupId, entity.GroupId);
        }

        [TestMethod]
        public async Task Test_GetGroupByIdAsync()
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
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<GroupService>();
            services.AddTransient<ILogger<GroupService>>(p =>
            {
                return logger;
            });
            services.AddTransient<GroupService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var GroupService = sp.GetRequiredService<GroupService>();
            AspNetUsers testUser = await CreateFromUserAsync(dbContext);
            DataAccess.Models.FairPlaySocialSchema.Group entity = new()
            {
                Name = "TestModel",
                Description = "Description",
                OwnerApplicationUserId = testUser.Id,
                TopicTag = "AT Tag"
            };
            await dbContext.Group.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.GroupId);
            var result = await GroupService.GetGroupByIdAsync(entity.GroupId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}
