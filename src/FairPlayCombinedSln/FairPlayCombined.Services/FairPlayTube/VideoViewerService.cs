using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.FairPlayTube.VideoViewer;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    public partial class VideoViewerService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<VideoViewerService> logger) : BaseService
    {
        public async Task<PaginationOfT<VideoViewerModel>> GetPaginatedVideoViewerInfoForUserIdAsync(
            PaginationRequest paginationRequest,
            string videoId,
            string userId,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {methodName}", nameof(GetPaginatedVideoViewerInfoForUserIdAsync));
            PaginationOfT<VideoViewerModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoWatchTime
                .Include(p=>p.VideoInfo)
                .Include(p=>p.WatchedByApplicationUser)
                .Where(p=>p.VideoInfo.ApplicationUserId == userId && p.VideoInfo.VideoId == videoId)
                .GroupBy(p=>new 
                { 
                    Username=p.WatchedByApplicationUser.UserName,
                    VideoId = p.VideoInfo.VideoId,
                    VideoName = p.VideoInfo.Name
                })
                .OrderByDescending(p=>p.Sum(x=>x.WatchTime))
                .Select(p => new VideoViewerModel
                {
                    TotalSessions = p.Count(),
                    TotalTimeWatched = p.Sum(p=>p.WatchTime),
                    Username = p.Key.Username,
                    VideoId = p.Key.VideoId,
                    VideoName = p.Key.VideoName
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
