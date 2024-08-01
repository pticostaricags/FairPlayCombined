using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class VideoDigitalMarketingPlanService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserFundService userFundService,
        IOpenAIService openAIService)
        : BaseService, IVideoDigitalMarketingPlanService
    {
        public async Task<string?> GetVideoDigitalMarketingPlanAsync(
            long videoInfoId,
            string socialNetworkName,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            var result = await dbContext.VideoDigitalMarketingPlan.Where(
                p => p.VideoInfoId == videoInfoId && p.SocialNetworkName == socialNetworkName)
                .Select(p => p.HtmlDigitalMarketingPlan)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return result;
        }
        public async Task SaveVideoDigitalMarketingPlanAsync(long videoInfoId,
            string htmlDigitalMarketingPlan,
            string socialNetworkName,
            bool replaceExistent,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            if (replaceExistent)
            {
                await dbContext.VideoDigitalMarketingPlan
                    .Where(p => p.VideoInfoId == videoInfoId)
                    .ExecuteDeleteAsync(cancellationToken: cancellationToken);
            }
            await dbContext.VideoDigitalMarketingPlan.AddAsync(new()
            {
                VideoInfoId = videoInfoId,
                HtmlDigitalMarketingPlan = htmlDigitalMarketingPlan,
                SocialNetworkName = socialNetworkName
            },
                cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task<string> CreateVideoDigitalMarketingPlanAsync(long videoInfoId, CancellationToken cancellationToken)
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
                Constants.PromptsNames.CreateDigitalMarketingIdeas, cancellationToken);
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
            var userMessage = $"Today's Date: {DateTimeOffset.UtcNow.Date}. Video Title: {videoDataEntity.Description}. Video Captions: {videoDataEntity.EnglishCaptions}";
            var result = await openAIService.GenerateChatCompletionAsync(promptEntity.BaseText,
                userMessage, cancellationToken);
            var resultText = result!.choices![0].message!.content;
            await dbContext.VideoDigitalMarketingPlan.AddAsync(new()
            {

                HtmlDigitalMarketingPlan = resultText,
                OpenAipromptId = result.OpenAIPromptId,
                SocialNetworkName = "LinkedIn",
                VideoInfoId = videoInfoId
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return resultText!;
        }
    }
}
