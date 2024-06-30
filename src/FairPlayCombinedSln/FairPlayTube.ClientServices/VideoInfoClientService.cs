using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace FairPlayTube.ClientServices
{
    public class VideoInfoClientService(
        [FromKeyedServices("AnonymousApiClient")]
        KiotaClient.ApiClient anonymousClient
        ) : IVideoInfoService
    {
        public Task<long> CreateVideoInfoAsync(CreateVideoInfoModel createModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVideoInfoByIdAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<VideoInfoModel[]> GetAllVideoInfoAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationOfT<VideoInfoModel>> GetPaginatedCompletedVideoInfoAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var result = await anonymousClient.Videoinfo.GetPaginatedCompletedVideoInfoAsync
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.StartIndex = paginationRequest.StartIndex;
                }, cancellationToken);
            return new PaginationOfT<VideoInfoModel>()
            {
                Items=result!.Items!.Select(p=>new  VideoInfoModel()
                {
                    VideoId = p.VideoId,
                    Name = p.Name,
                    LifetimeSessions=p.LifetimeSessions!.Value,
                    LifetimeViewers=p.LifetimeViewers!.Value,
                    LifetimeWatchTime=TimeSpan.Parse(p.LifetimeWatchTime!),
                    PublishedOnString=p.PublishedOnString
                }).ToArray(),
                PageSize=result.PageSize!.Value,
                TotalItems=result.TotalItems!.Value,
                TotalPages=result.TotalPages!.Value,
            };
        }

        public Task<PaginationOfT<VideoInfoModel>> GetPaginatedCompletedVideoInfobyUserIdAsync(PaginationRequest paginationRequest, string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationOfT<VideoInfoModel>> GetPaginatedNotCompletedVideoInfobyUserIdAsync(PaginationRequest paginationRequest, string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationOfT<VideoInfoModel>> GetPaginatedVideoInfoAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<VideoInfoModel> GetVideoInfoByIdAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<VideoInfoModel?> GetVideoInfoByVideoIdAsync(string videoId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
