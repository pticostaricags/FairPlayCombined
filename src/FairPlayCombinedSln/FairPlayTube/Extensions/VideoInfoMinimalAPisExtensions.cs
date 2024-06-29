using FairPlayCombined.Common;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace FairPlayTube.Extensions
{
    public static class VideoInfoMinimalAPisExtensions
    {
        public static WebApplication AddVideoInfoEndpoints(this WebApplication app)
        {
            app.MapGet("/videoinfo/GetPaginatedCompletedVideoInfoAsync",
                async ([FromServices] IVideoInfoService videoInfoService,
                int startIndex, CancellationToken cancellationToken) =>
                {
                    PaginationRequest paginationRequest = new()
                    {
                        StartIndex = startIndex,
                        PageSize = Constants.Pagination.PageSize,
                        SortingItems = [
                            new SortingItem()
                            {
                                PropertyName=nameof(VideoInfoModel.VideoInfoId),
                                SortType= SortType.Descending
                            }]
                    };
                    var result = await videoInfoService
                    .GetPaginatedCompletedVideoInfoAsync(paginationRequest, cancellationToken);
                    return result;
                });
            return app;
        }
    }
}
