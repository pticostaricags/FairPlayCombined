using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.FairPlayBlogs.Blog;
using FairPlayCombined.Services.FairPlayBlogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayBlogs
{
    [TestClass]
    public class BlogServiceTests: ServicesBase
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
            services.AddTransient<BlogService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleBlog in dbContext.Blog)
            {
                dbContext.Blog.Remove(singleBlog);
            }
            foreach (var singlePhoto in dbContext.Photo)
            {
                dbContext.Photo.Remove(singlePhoto);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateBlogAsync()
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
            var logger = loggerFactory!.CreateLogger<BlogService>();
            services.AddTransient<ILogger<BlogService>>(p =>
            {
                return logger;
            });
            services.AddTransient<BlogService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var blogService = sp.GetRequiredService<BlogService>();
            var userEntity = await ServicesBase.CreateFromUserAsync(dbContext);
            Photo photo = new()
            {
                Name = nameof(Properties.Resources.TestProduct),
                Filename = $"{Properties.Resources.TestProduct}.bmp",
                PhotoBytes = Properties.Resources.TestProduct,
            };
            await dbContext.Photo.AddAsync(photo);
            await dbContext.SaveChangesAsync();
            CreateBlogModel createBlogModel = new()
            {
                Description = "Test Blog Desc",
                IsCustomDomainVerified = false,
                Name = "Test Blog Name",
                OwnerApplicationUserId = userEntity.Id,
                HeaderPhotoId = photo.PhotoId
            };
            TestUserProviderService.CurrentUserId = userEntity.Id;
            var blogId = await blogService.CreateBlogAsync(createBlogModel, CancellationToken.None);
            var blogEntity = await dbContext.Blog.SingleAsync(p => p.BlogId == blogId);
            Assert.AreEqual(blogId, blogEntity.BlogId);
        }
    }
}
