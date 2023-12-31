﻿using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Gender;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class GenderServiceTests : ServicesBase
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
            services.AddTransient<GenderService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleGender in dbContext.Gender)
            {
                dbContext.Gender.Remove(singleGender);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateGenderAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<GenderService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var GenderService = sp.GetRequiredService<GenderService>();
            CreateGenderModel createGenderModel = new CreateGenderModel()
            {
                Name = "TestModel"
            };
            await GenderService.CreateGenderAsync(createGenderModel, CancellationToken.None);
            var result = await dbContext.Gender.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createGenderModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteGenderAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<GenderService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var GenderService = sp.GetRequiredService<GenderService>();
            Gender entity = new Gender()
            {
                Name = "TestModel"
            };
            await dbContext.Gender.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.GenderId);
            await GenderService.DeleteGenderByIdAsync(entity.GenderId, CancellationToken.None);
            var itemsCount = await dbContext.Gender.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedGenderAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<GenderService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var GenderService = sp.GetRequiredService<GenderService>();
            Gender entity = new Gender()
            {
                Name = "TestModel"
            };
            await dbContext.Gender.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.GenderId);
            var result = await GenderService.GetPaginatedGenderAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new SortingItem()
                        {
                            PropertyName = nameof(GenderModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].GenderId, entity.GenderId);
        }

        [TestMethod]
        public async Task Test_GetGenderByIdAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction=>sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<GenderService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var GenderService = sp.GetRequiredService<GenderService>();
            Gender entity = new Gender()
            {
                Name = "TestModel"
            };
            await dbContext.Gender.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.GenderId);
            var result = await GenderService.GetGenderByIdAsync(entity.GenderId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}
