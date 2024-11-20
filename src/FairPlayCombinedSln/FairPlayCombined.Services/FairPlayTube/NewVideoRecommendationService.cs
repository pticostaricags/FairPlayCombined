using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Text;
using FairPlayCombined.Common;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateNewVideoRecommendationModel,
        UpdateNewVideoRecommendationModel,
        NewVideoRecommendationModel,
        FairPlayCombinedDbContext,
        NewVideoRecommendation,
        PaginationRequest,
        PaginationOfT<NewVideoRecommendationModel>
        >]
    public partial class NewVideoRecommendationService : BaseService, INewVideoRecommendationService
    {
        private readonly IUserFundService? userFundService;
        private readonly IOpenAIService? openAIService;
        private readonly IUserProviderService? userProviderService;

        public NewVideoRecommendationService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserFundService userFundService,
        IOpenAIService openAIService,
        IUserProviderService userProviderService,
        ILogger<NewVideoRecommendationService> logger) : this(dbContextFactory, logger)
        {
            this.userFundService = userFundService;
            this.openAIService = openAIService;
            this.userProviderService = userProviderService;
        }
        public async Task<string> GenerateNewVideoRecommendationAsync(string languageCode,
            CancellationToken cancellationToken)
        {
            var hasRequiredFunds = await userFundService!.HasFundsToCreateDailyPostsAsync(cancellationToken);
            if (!hasRequiredFunds)
            {
                string message = "You don't have available funds left to perform the operation.";
                throw new RuleException(message);
            }
            var userId = userProviderService!.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var promptEntity = await dbContext.Prompt
                .AsNoTracking()
                .SingleAsync(p => p.PromptName ==
                Constants.PromptsNames.CreateNewVideoRecommendationIdea, cancellationToken);
            var videosDataEntity = await dbContext.VideoInfo
                .AsNoTracking()
                .AsSplitQuery()
                .Where(p => p.ApplicationUserId == userId)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    VideoKeywords = p.VideoKeyword.Select(p => p.Keyword),
                    VideoTopics = p.VideoTopic.Select(p => p.Topic)
                })
                .ToArrayAsync(cancellationToken);
            var titles = videosDataEntity!.Select(p => $"* Title: {p.Name}. Keywords: {String.Join(",", p.VideoKeywords!)}. Topics: {String.Join(",", p.VideoTopics!)}.\r\n");
            var userMessage = $"Video Titles: {String.Join(".", titles)}.";
            StringBuilder promptBuilder = new();
            promptBuilder.AppendLine(promptEntity!.BaseText!);
            promptBuilder.AppendLine($"The result must be in language culture: {languageCode}");
            logger.LogInformation("Invoking Chat Completion. User Message Size: {UserMessageSize}", userMessage.Length);
            var result = await openAIService!.GenerateChatCompletionAsync(promptBuilder.ToString(),
                userMessage, cancellationToken);
            var resultText = result!.choices![0].message!.content;
            await dbContext.NewVideoRecommendation.AddAsync(new()
            {
                HtmlNewVideoRecommendation = resultText,
                ApplicationUserId = userId
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return resultText!;
        }

        public async Task<PaginationOfT<NewVideoRecommendationModel>> GetPaginatedNewVideoRecommendationForUserIdAsync(PaginationRequest paginationRequest, string userId, CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedNewVideoRecommendationAsync));
            PaginationOfT<NewVideoRecommendationModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.NewVideoRecommendation
                .Where(p=>p.ApplicationUserId == userId)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(p => new FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation.NewVideoRecommendationModel
                {
                    NewVideoRecommendationId = p.NewVideoRecommendationId,
                    ApplicationUserId = p.ApplicationUserId,
                    HtmlNewVideoRecommendation = p.HtmlNewVideoRecommendation,

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
