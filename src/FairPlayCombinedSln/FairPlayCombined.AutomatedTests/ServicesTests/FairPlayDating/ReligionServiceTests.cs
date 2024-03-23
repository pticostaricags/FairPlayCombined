using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Religion;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class ReligionServiceTests : ServicesBase
    {
        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<ReligionService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleReligion in dbContext.Religion)
            {
                dbContext.Religion.Remove(singleReligion);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateReligionAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<ReligionService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var ReligionService = sp.GetRequiredService<ReligionService>();
            CreateReligionModel createReligionModel = new()
            {
                Name = "TestModel"
            };
            await ReligionService.CreateReligionAsync(createReligionModel, CancellationToken.None);
            var result = await dbContext.Religion.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createReligionModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteReligionAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<ReligionService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var ReligionService = sp.GetRequiredService<ReligionService>();
            Religion entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.Religion.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.ReligionId);
            await ReligionService.DeleteReligionByIdAsync(entity.ReligionId, CancellationToken.None);
            var itemsCount = await dbContext.Religion.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedReligionAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<ReligionService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var ReligionService = sp.GetRequiredService<ReligionService>();
            Religion entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.Religion.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.ReligionId);
            var result = await ReligionService.GetPaginatedReligionAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems =
                    [
                        new SortingItem()
                        {
                            PropertyName = nameof(ReligionModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    ]
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].ReligionId, entity.ReligionId);
        }

        [TestMethod]
        public async Task Test_GetReligionByIdAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<ReligionService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var ReligionService = sp.GetRequiredService<ReligionService>();
            Religion entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.Religion.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.ReligionId);
            var result = await ReligionService.GetReligionByIdAsync(entity.ReligionId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}
