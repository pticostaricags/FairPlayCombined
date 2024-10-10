using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoThumbnail;
using FairPlayCombined.Models.OpenAI;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Text;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
    CreateVideoThumbnailModel,
    UpdateVideoThumbnailModel,
    VideoThumbnailModel,
    FairPlayCombinedDbContext,
    VideoThumbnail,
    PaginationRequest,
    PaginationOfT<VideoThumbnailModel>
    >]
    public partial class VideoThumbnailService : BaseService, IVideoThumbnailService
    {
        private readonly IUserFundService? userFundService;
        public VideoThumbnailService(
            IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
            ILogger<VideoThumbnailService> logger,
            IUserFundService userFundService):this(dbContextFactory, logger)
        {
            this.userFundService = userFundService;
        }
        public async Task<GenerateDallE3ResponseModel?> GenerateVideoThumbnailAsync(
            long videoInfoId,
            IOpenAIService openAIService,
            int imageStyleId,
            bool requestCleanThumbnail,
            HttpClient httpClient,
            CancellationToken cancellationToken)
        {
            var hasRequiredFunds = await userFundService!.HasFundsToCreateThumbnailsAsync(cancellationToken);
            if (!hasRequiredFunds)
            {
                string message = "You don't have available funds left to perform the operation.";
                throw new RuleException(message);
            }
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var promptEntity = await dbContext.Prompt
                .AsNoTracking()
                .SingleAsync(p => p.PromptName ==
                Constants.PromptsNames.CreateYouTubeThumbnail, cancellationToken);
            var videoDataEntity = await dbContext.VideoInfo
                .AsNoTracking()
                .Where(p => p.VideoInfoId == videoInfoId)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    EnglishCaptions = p.VideoCaptions.Single(p => p.Language == "en-US").Content
                })
                .SingleAsync(cancellationToken);
            var imageStyle = await dbContext.ImageStyle.SingleAsync(p => p.ImageStyleId == imageStyleId, cancellationToken);
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($"{promptEntity!.BaseText}.");
            stringBuilder.AppendLine($"Make sure the image style is: {imageStyle.StyleName}, this is mandatory.");
            if (requestCleanThumbnail)
            {
                stringBuilder.AppendLine("Make sure the image does not have any text nor typography.");
            }
            stringBuilder.AppendLine($"Video Title: {videoDataEntity.Name}.");
            stringBuilder.AppendLine($"Video Description: {videoDataEntity.Description}.");
            stringBuilder.AppendLine($"Video Captions: {videoDataEntity.EnglishCaptions}.");
            string prompt = stringBuilder.ToString();
            if (prompt.Length > 4000)
                prompt = prompt[..4000];
            var result = await openAIService.GenerateDallE3ImageAsync(prompt, cancellationToken);
            Photo photoEntity = new()
            {
                Filename = $"Video-{videoInfoId}-thumbnail.jpg",
                Name = $"Video-{videoInfoId}-thumbnail",
                PhotoBytes = await httpClient
                .GetByteArrayAsync(result!.data![0].url, cancellationToken)
            };
            await dbContext.VideoThumbnail
                .AddAsync(new()
                {
                    OpenAipromptId = result.OpenAIPromptId,
                    VideoInfoId = videoInfoId,
                    Photo = photoEntity,
                }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<PaginationOfT<VideoThumbnailModel>>
            GetPaginatedVideoThumbnailByVideoInfoIdAsync(long videoInfoId,
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedVideoThumbnailAsync));
            PaginationOfT<VideoThumbnailModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoThumbnail
                .AsNoTracking()
                .AsSplitQuery()
                .Where(p => p.VideoInfoId == videoInfoId)
                .Select(p => new FairPlayCombined.Models.FairPlayTube.VideoThumbnail.VideoThumbnailModel
                {
                    VideoThumbnailId = p.VideoThumbnailId,
                    VideoInfoId = p.VideoInfoId,
                    PhotoId = p.PhotoId,
                    ThumbnailCost = p.OpenAiprompt.OperationCost
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
