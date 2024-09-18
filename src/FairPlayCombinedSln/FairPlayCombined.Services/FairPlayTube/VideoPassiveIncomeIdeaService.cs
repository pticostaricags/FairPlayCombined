using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class VideoPassiveIncomeIdeaService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserFundService userFundService,
        IOpenAIService openAIService)
        : BaseService, IVideoPassiveIncomeIdeaService
    {
        public async Task<string> CreateVideoPassiveIncomeIdeaAsync(long videoInfoId, string languageCode, CancellationToken cancellationToken)
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
                Constants.PromptsNames.CreateVideoPassiveIncomeStrategy, cancellationToken);
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
            var userMessage = $"Video Title: {videoDataEntity.Description}. Video Captions: {videoDataEntity.EnglishCaptions}";
            StringBuilder promptBuilder = new();
            promptBuilder.AppendLine(promptEntity!.BaseText!);
            promptBuilder.AppendLine($"The result must be in language culture: {languageCode}");
            var result = await openAIService.GenerateChatCompletionAsync(promptBuilder.ToString(),
                userMessage, cancellationToken);
            var resultText = result!.choices![0].message!.content;
            await dbContext.VideoPassiveIncome.AddAsync(new()
            {

                HtmlPassiveIncomeIdea = resultText,
                OpenAipromptId = result.OpenAIPromptId,
                VideoInfoId = videoInfoId
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return resultText!;
        }
    }
}
