using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoDigitalMarketingDailyPosts;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Text;

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
        private readonly IUserFundService userFundService;
        private readonly IOpenAIService openAIService;
        public VideoDigitalMarketingDailyPostsService(
            IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
            ILogger<VideoDigitalMarketingDailyPostsService> logger,
            IUserFundService userFundService,
            IOpenAIService openAIService) : this(dbContextFactory, logger)
        {
            this.userFundService = userFundService;
            this.openAIService = openAIService;
        }
        public async Task<string> CreateVideoDigitalMarketingDailyPostsForLinkedInAsync
            (long videoInfoId, string languageCode, CancellationToken cancellationToken)
        {
            var hasRequiredFunds = await userFundService.HasFundsToCreateDailyPostsAsync(cancellationToken);
            if (!hasRequiredFunds)
            {
                string message = "You don't have available funds left to perform the operation.";
                throw new RuleException(message);
            }
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var promptEntity = await dbContext.Prompt
                .AsNoTracking()
                .SingleAsync(p => p.PromptName ==
                Constants.PromptsNames.CreateVideoDailyPosts, cancellationToken);
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
            StringBuilder promptBuilder = new(promptEntity.BaseText);
            promptBuilder.AppendLine($"The posts must be in language culture: {languageCode}");
            var userMessage = $"Today's Date: {DateTimeOffset.UtcNow.Date}. Video Title: {videoDataEntity.Description}. Video Captions: {videoDataEntity.EnglishCaptions}";
            var result = await this.openAIService.GenerateChatCompletionAsync(promptBuilder.ToString(),
                userMessage, cancellationToken);
            var resultText = result!.choices![0].message!.content;
            await dbContext.VideoDigitalMarketingDailyPosts.AddAsync(new()
            {
                HtmlVideoDigitalMarketingDailyPostsIdeas = resultText,
                OpenAipromptId = result.OpenAIPromptId,
                SocialNetworkName = "LinkedIn",
                VideoInfoId = videoInfoId
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return resultText!;
        }

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
                .Where(p => p.VideoInfoId == videoInfoId)
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
