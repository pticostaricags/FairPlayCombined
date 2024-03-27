using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Frequency;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class FrequencyServiceTests : ServicesBase
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
            services.AddTransient<FrequencyService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleFrequency in dbContext.Frequency)
            {
                dbContext.Frequency.Remove(singleFrequency);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateFrequencyAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<FrequencyService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var FrequencyService = sp.GetRequiredService<FrequencyService>();
            CreateFrequencyModel createFrequencyModel = new()
            {
                Name = "TestModel"
            };
            await FrequencyService.CreateFrequencyAsync(createFrequencyModel, CancellationToken.None);
            var result = await dbContext.Frequency.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createFrequencyModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteFrequencyAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<FrequencyService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var FrequencyService = sp.GetRequiredService<FrequencyService>();
            Frequency entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.Frequency.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.FrequencyId);
            await FrequencyService.DeleteFrequencyByIdAsync(entity.FrequencyId, CancellationToken.None);
            var itemsCount = await dbContext.Frequency.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedFrequencyAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<FrequencyService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var FrequencyService = sp.GetRequiredService<FrequencyService>();
            Frequency entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.Frequency.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.FrequencyId);
            var result = await FrequencyService.GetPaginatedFrequencyAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems =
                    [
                        new SortingItem()
                        {
                            PropertyName = nameof(FrequencyModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    ]
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].FrequencyId, entity.FrequencyId);
        }

        [TestMethod]
        public async Task Test_GetFrequencyByIdAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<FrequencyService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var FrequencyService = sp.GetRequiredService<FrequencyService>();
            Frequency entity = new()
            {
                Name = "TestModel"
            };
            await dbContext.Frequency.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.FrequencyId);
            var result = await FrequencyService.GetFrequencyByIdAsync(entity.FrequencyId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}
