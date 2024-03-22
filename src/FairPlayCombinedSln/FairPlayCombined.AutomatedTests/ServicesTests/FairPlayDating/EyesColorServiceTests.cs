using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.EyesColor;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class EyesColorServiceTests : ServicesBase
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
            services.AddTransient<EyesColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleEyesColor in dbContext.EyesColor)
            {
                dbContext.EyesColor.Remove(singleEyesColor);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateEyesColorAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<EyesColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var EyesColorService = sp.GetRequiredService<EyesColorService>();
            CreateEyesColorModel createEyesColorModel = new()
            {
                Name = "TestModel"
            };
            await EyesColorService.CreateEyesColorAsync(createEyesColorModel, CancellationToken.None);
            var result = await dbContext.EyesColor.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createEyesColorModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteEyesColorAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<EyesColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var EyesColorService = sp.GetRequiredService<EyesColorService>();
            EyesColor entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.EyesColor.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.EyesColorId);
            await EyesColorService.DeleteEyesColorByIdAsync(entity.EyesColorId, CancellationToken.None);
            var itemsCount = await dbContext.EyesColor.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedEyesColorAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<EyesColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var EyesColorService = sp.GetRequiredService<EyesColorService>();
            EyesColor entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.EyesColor.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.EyesColorId);
            var result = await EyesColorService.GetPaginatedEyesColorAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems =
                    [
                        new SortingItem()
                        {
                            PropertyName = nameof(EyesColorModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    ]
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].EyesColorId, entity.EyesColorId);
        }

        [TestMethod]
        public async Task Test_GetEyesColorByIdAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<EyesColorService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var EyesColorService = sp.GetRequiredService<EyesColorService>();
            EyesColor entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.EyesColor.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.EyesColorId);
            var result = await EyesColorService.GetEyesColorByIdAsync(entity.EyesColorId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}
