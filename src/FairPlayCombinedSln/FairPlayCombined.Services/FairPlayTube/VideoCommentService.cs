using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Models.FairPlayTube.VideoComment;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateVideoCommentModel,
        UpdateVideoCommentModel,
        VideoCommentModel,
        FairPlayCombinedDbContext,
        VideoComment,
        PaginationRequest,
        PaginationOfT<VideoCommentModel>
        >]
    public partial class VideoCommentService : BaseService
    {
        public async Task<PaginationOfT<VideoCommentModel>> GetPaginatedCommentsByVideonIdAsync(
            PaginationRequest paginationRequest,
            string videoId,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedCommentsByVideonIdAsync));
            PaginationOfT<VideoCommentModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoComment
                .Include(p=>p.VideoInfo)
                .Where(p => p.VideoInfo.VideoId == videoId)
                .Select(p => new VideoCommentModel
                {
                    ApplicationUserId = p.ApplicationUserId,
                    Comment = p.Comment,
                    RowCreationDateTime=p.RowCreationDateTime,
                    RowCreationUser=p.RowCreationUser,
                    VideoCommentId=p.VideoCommentId,
                    VideoInfoId=p.VideoInfoId
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
    }
}
