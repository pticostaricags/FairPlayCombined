using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoDigitalMarketingDailyPosts;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateVideoDigitalMarketingDailyPostsModel,
        UpdateVideoDigitalMarketingDailyPostsModel,
        VideoDigitalMarketingDailyPostsModel,
        FairPlayCombinedDbContext,
        VideoDigitalMarketingDailyPosts,
        PaginationRequest,
        PaginationOfT<VideoDigitalMarketingDailyPostsModel>
        >]
    public partial class VideoDigitalMarketingDailyPostsService : BaseService, IVideoDigitalMarketingDailyPostsService
    {
        public async Task<PaginationOfT<VideoDigitalMarketingDailyPostsModel>> GetPaginatedVideoDigitalMarketingDailyPostsByVideoInfoIdAsync(long videoInfoId, PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedVideoDigitalMarketingDailyPostsAsync));
            PaginationOfT<VideoDigitalMarketingDailyPostsModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoDigitalMarketingDailyPosts
                .Where(p=>p.VideoInfoId == videoInfoId)
                .Select(p => new FairPlayCombined.Models.FairPlayTube.VideoDigitalMarketingDailyPosts.VideoDigitalMarketingDailyPostsModel
                {
                    VideoDigitalMarketingDailyPostsId = p.VideoDigitalMarketingDailyPostsId,
                    VideoInfoId = p.VideoInfoId,
                    SocialNetworkName = p.SocialNetworkName,
                    HtmlVideoDigitalMarketingDailyPostsIdeas = p.HtmlVideoDigitalMarketingDailyPostsIdeas,

                });
            if (!String.IsNullOrEmpty(orderByString))
                query = query.OrderBy(orderByString);
            result.TotalItems = await query.CountAsync(cancellationToken);
            result.PageSize = paginationRequest.PageSize;
            result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems / result.PageSize);
            result.Items = await query
            .Skip(paginationRequest.StartIndex)
            .Take(paginationRequest.PageSize)
            .ToArrayAsync(cancellationToken);
            return result;
        }

        public async Task<string?> GetVideoDigitalMarketingDailyPostsAsync(
            long videoInfoId,
            string socialNetworkName,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            var result = await dbContext.VideoDigitalMarketingDailyPosts.Where(
                p => p.VideoInfoId == videoInfoId && p.SocialNetworkName == socialNetworkName)
                .Select(p => p.HtmlVideoDigitalMarketingDailyPostsIdeas)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return result;
        }
        public async Task SaveVideoDigitalMarketingDailyPostsAsync(long videoInfoId,
            string htmlVideoDigitalMarketingDailyPostsIdeas,
            string socialNetworkName,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            await dbContext.VideoDigitalMarketingDailyPosts.AddAsync(new()
            {
                VideoInfoId = videoInfoId,
                HtmlVideoDigitalMarketingDailyPostsIdeas = htmlVideoDigitalMarketingDailyPostsIdeas,
                SocialNetworkName = socialNetworkName
            },
                cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }
    }
}
