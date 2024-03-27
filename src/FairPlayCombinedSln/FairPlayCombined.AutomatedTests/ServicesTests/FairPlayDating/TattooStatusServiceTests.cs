using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.TattooStatus;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class TattooStatusServiceTests : ServicesBase
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
            services.AddTransient<TattooStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleTattooStatus in dbContext.TattooStatus)
            {
                dbContext.TattooStatus.Remove(singleTattooStatus);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateTattooStatusAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<TattooStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var TattooStatusService = sp.GetRequiredService<TattooStatusService>();
            CreateTattooStatusModel createTattooStatusModel = new()
            {
                Name = "TestModel"
            };
            await TattooStatusService.CreateTattooStatusAsync(createTattooStatusModel, CancellationToken.None);
            var result = await dbContext.TattooStatus.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createTattooStatusModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteTattooStatusAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<TattooStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var TattooStatusService = sp.GetRequiredService<TattooStatusService>();
            TattooStatus entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.TattooStatus.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.TattooStatusId);
            await TattooStatusService.DeleteTattooStatusByIdAsync(entity.TattooStatusId, CancellationToken.None);
            var itemsCount = await dbContext.TattooStatus.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedTattooStatusAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<TattooStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var TattooStatusService = sp.GetRequiredService<TattooStatusService>();
            TattooStatus entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.TattooStatus.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.TattooStatusId);
            var result = await TattooStatusService.GetPaginatedTattooStatusAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems =
                    [
                        new SortingItem()
                        {
                            PropertyName = nameof(TattooStatusModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    ]
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].TattooStatusId, entity.TattooStatusId);
        }

        [TestMethod]
        public async Task Test_GetTattooStatusByIdAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<TattooStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var TattooStatusService = sp.GetRequiredService<TattooStatusService>();
            TattooStatus entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.TattooStatus.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.TattooStatusId);
            var result = await TattooStatusService.GetTattooStatusByIdAsync(entity.TattooStatusId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}
