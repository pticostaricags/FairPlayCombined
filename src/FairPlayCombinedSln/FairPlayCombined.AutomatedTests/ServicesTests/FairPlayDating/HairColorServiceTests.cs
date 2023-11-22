using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.HairColor;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class HairColorServiceTests : ServicesBase
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
            services.AddTransient<HairColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleHairColor in dbContext.HairColor)
            {
                dbContext.HairColor.Remove(singleHairColor);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateHairColorAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<HairColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var HairColorService = sp.GetRequiredService<HairColorService>();
            CreateHairColorModel createHairColorModel = new CreateHairColorModel()
            {
                Name = "TestModel"
            };
            await HairColorService.CreateHairColorAsync(createHairColorModel, CancellationToken.None);
            var result = await dbContext.HairColor.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createHairColorModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteHairColorAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<HairColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var HairColorService = sp.GetRequiredService<HairColorService>();
            HairColor entity = new HairColor()
            {
                Name = "TestModel"
            };
            await dbContext.HairColor.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.HairColorId);
            await HairColorService.DeleteHairColorByIdAsync(entity.HairColorId, CancellationToken.None);
            var itemsCount = await dbContext.HairColor.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedHairColorAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<HairColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var HairColorService = sp.GetRequiredService<HairColorService>();
            HairColor entity = new HairColor()
            {
                Name = "TestModel"
            };
            await dbContext.HairColor.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.HairColorId);
            var result = await HairColorService.GetPaginatedHairColorAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new SortingItem()
                        {
                            PropertyName = nameof(HairColorModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].HairColorId, entity.HairColorId);
        }

        [TestMethod]
        public async Task Test_GetHairColorByIdAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<HairColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var HairColorService = sp.GetRequiredService<HairColorService>();
            HairColor entity = new HairColor()
            {
                Name = "TestModel"
            };
            await dbContext.HairColor.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.HairColorId);
            var result = await HairColorService.GetHairColorByIdAsync(entity.HairColorId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}
