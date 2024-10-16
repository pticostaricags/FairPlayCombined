using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.Models.FairPlaySocial.Post;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlaySocial;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class PostServiceTests : ServicesBase
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
            services.AddTransient<PostService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singlePost in dbContext.Post)
            {
                dbContext.Post.Remove(singlePost);
            }
            foreach (var singlePostType in dbContext.PostType)
            {
                dbContext.PostType.Remove(singlePostType);
            }
            foreach (var singlePostVisibility in dbContext.PostVisibility)
            {
                dbContext.PostVisibility.Remove(singlePostVisibility);
            }
            foreach (var singleUser in dbContext.AspNetUsers)
            {
                dbContext.AspNetUsers.Remove(singleUser);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreatePostAsync()
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
            var logger = loggerFactory!.CreateLogger<PostService>();
            services.AddTransient<ILogger<PostService>>(p =>
            {
                return logger;
            });
            services.AddTransient<PostService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            string testUserName = "fromuser@test.test";
            AspNetUsers testUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = testUserName,
                NormalizedUserName = testUserName.Normalize(),
                Email = testUserName,
                NormalizedEmail = testUserName.Normalize(),
                Name = "AT FROM NAME",
                Lastname = "AT FROM LASTNAME"
            };
            await dbContext.AspNetUsers.AddAsync(testUser);
            PostType postType = new()
            {
                Name = "AT Type"
            };
            await dbContext.PostType.AddAsync(postType);
            PostVisibility postVisibility = new()
            {
                Name = "AT Name",
                Description = "AT Desc",
            };
            await dbContext.PostVisibility.AddAsync(postVisibility);
            await dbContext.SaveChangesAsync();
            var PostService = sp.GetRequiredService<PostService>();

            CreatePostModel createPostModel = new()
            {
                OwnerApplicationUserId = testUser.Id,
                PostTypeId = postType.PostTypeId,
                PostVisibilityId = postVisibility.PostVisibilityId,
                Text = "Test Text"
            };
            await PostService.CreatePostAsync(createPostModel, CancellationToken.None);
            var result = await dbContext.Post.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createPostModel.Text, result.Text);
        }

        [TestMethod]
        public async Task Test_DeletePostAsync()
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
            var logger = loggerFactory!.CreateLogger<PostService>();
            services.AddTransient<ILogger<PostService>>(p =>
            {
                return logger;
            });
            services.AddTransient<PostService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var PostService = sp.GetRequiredService<PostService>();
            string testUserName = "fromuser@test.test";
            AspNetUsers testUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = testUserName,
                NormalizedUserName = testUserName.Normalize(),
                Email = testUserName,
                NormalizedEmail = testUserName.Normalize(),
                Name = "AT FROM NAME",
                Lastname = "AT FROM LASTNAME"
            };
            await dbContext.AspNetUsers.AddAsync(testUser);
            await dbContext.SaveChangesAsync();
            PostType postType = new()
            {
                Name = "AT Type"
            };
            await dbContext.PostType.AddAsync(postType);
            PostVisibility postVisibility = new()
            {
                Name = "AT Name",
                Description = "AT Desc",
            };
            await dbContext.PostVisibility.AddAsync(postVisibility);
            await dbContext.SaveChangesAsync();
            Post entity = new()
            {
                OwnerApplicationUserId = testUser.Id,
                PostTypeId = postType.PostTypeId,
                PostVisibilityId = postVisibility.PostVisibilityId,
                Text = "Test Text"
            };
            await dbContext.Post.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.PostId);
            await PostService.DeletePostByIdAsync(entity.PostId, CancellationToken.None);
            var itemsCount = await dbContext.Post.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedPostAsync()
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
            var logger = loggerFactory!.CreateLogger<PostService>();
            services.AddTransient<ILogger<PostService>>(p =>
            {
                return logger;
            });
            services.AddTransient<PostService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var PostService = sp.GetRequiredService<PostService>();
            string testUserName = "fromuser@test.test";
            AspNetUsers testUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = testUserName,
                NormalizedUserName = testUserName.Normalize(),
                Email = testUserName,
                NormalizedEmail = testUserName.Normalize(),
                Name = "AT FROM NAME",
                Lastname = "AT FROM LASTNAME"
            };
            await dbContext.AspNetUsers.AddAsync(testUser);
            await dbContext.SaveChangesAsync();
            PostType postType = new()
            {
                Name = "AT Type"
            };
            await dbContext.PostType.AddAsync(postType);
            PostVisibility postVisibility = new()
            {
                Name = "AT Name",
                Description = "AT Desc",
            };
            await dbContext.PostVisibility.AddAsync(postVisibility);
            await dbContext.SaveChangesAsync();
            Post entity = new()
            {
                OwnerApplicationUserId = testUser.Id,
                PostTypeId = postType.PostTypeId,
                PostVisibilityId = postVisibility.PostVisibilityId,
                Text = "Test Text"
            };
            await dbContext.Post.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.PostId);
            var result = await PostService.GetPaginatedPostAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new()
                        {
                            PropertyName = nameof(Post.PostId),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].PostId, entity.PostId);
        }

        [TestMethod]
        public async Task Test_GetPostByIdAsync()
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
            var logger = loggerFactory!.CreateLogger<PostService>();
            services.AddTransient<ILogger<PostService>>(p =>
            {
                return logger;
            });
            services.AddTransient<PostService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var PostService = sp.GetRequiredService<PostService>();
            string testUserName = "fromuser@test.test";
            AspNetUsers testUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = testUserName,
                NormalizedUserName = testUserName.Normalize(),
                Email = testUserName,
                NormalizedEmail = testUserName.Normalize(),
                Name = "AT FROM NAME",
                Lastname = "AT FROM LASTNAME"
            };
            await dbContext.AspNetUsers.AddAsync(testUser);
            await dbContext.SaveChangesAsync();
            PostType postType = new()
            {
                Name = "AT Type"
            };
            await dbContext.PostType.AddAsync(postType);
            PostVisibility postVisibility = new()
            {
                Name = "AT Name",
                Description = "AT Desc",
            };
            await dbContext.PostVisibility.AddAsync(postVisibility);
            await dbContext.SaveChangesAsync();
            Post entity = new()
            {
                OwnerApplicationUserId = testUser.Id,
                PostTypeId = postType.PostTypeId,
                PostVisibilityId = postVisibility.PostVisibilityId,
                Text = "Test Text"
            };
            await dbContext.Post.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.PostId);
            var result = await PostService.GetPostByIdAsync(entity.PostId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Text, result.Text);
        }
    }
}
