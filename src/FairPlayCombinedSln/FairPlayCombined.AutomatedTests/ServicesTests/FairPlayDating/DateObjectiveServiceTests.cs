using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.DateObjective;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class DateObjectiveServiceTests : ServicesBase
    {

        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<DateObjectiveService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleDateObjective in dbContext.DateObjective)
            {
                dbContext.DateObjective.Remove(singleDateObjective);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateDateObjectiveAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<DateObjectiveService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var DateObjectiveService = sp.GetRequiredService<DateObjectiveService>();
            CreateDateObjectiveModel createDateObjectiveModel = new CreateDateObjectiveModel()
            {
                Name = "TestModel"
            };
            await DateObjectiveService.CreateDateObjectiveAsync(createDateObjectiveModel, CancellationToken.None);
            var result = await dbContext.DateObjective.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createDateObjectiveModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteDateObjectiveAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<DateObjectiveService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var DateObjectiveService = sp.GetRequiredService<DateObjectiveService>();
            DateObjective entity = new DateObjective()
            {
                Name = "TestModel"
            };
            await dbContext.DateObjective.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.DateObjectiveId);
            await DateObjectiveService.DeleteDateObjectiveByIdAsync(entity.DateObjectiveId, CancellationToken.None);
            var itemsCount = await dbContext.DateObjective.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedDateObjectiveAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<DateObjectiveService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var DateObjectiveService = sp.GetRequiredService<DateObjectiveService>();
            DateObjective entity = new DateObjective()
            {
                Name = "TestModel"
            };
            await dbContext.DateObjective.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.DateObjectiveId);
            var result = await DateObjectiveService.GetPaginatedDateObjectiveAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new SortingItem()
                        {
                            PropertyName = nameof(DateObjectiveModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].DateObjectiveId, entity.DateObjectiveId);
        }

        [TestMethod]
        public async Task Test_GetDateObjectiveByIdAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<DateObjectiveService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var DateObjectiveService = sp.GetRequiredService<DateObjectiveService>();
            DateObjective entity = new DateObjective()
            {
                Name = "TestModel"
            };
            await dbContext.DateObjective.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.DateObjectiveId);
            var result = await DateObjectiveService.GetDateObjectiveByIdAsync(entity.DateObjectiveId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}
