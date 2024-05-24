using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.Promp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Services.Common
{
    public class PromptGeneratorService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<PromptGeneratorService> logger) : IPromptGeneratorService
    {
        public async Task<PromptModel[]> GetAllPromptsAsync(CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.Prompt.Select(p => new PromptModel()
            {
                BaseText = p.BaseText,
                PromptId = p.PromptId,
                PromptName = p.PromptName
            }).ToArrayAsync(cancellationToken);
            return result;
        }

        public async Task<bool> UpdateAllPromptsAsync(List<PromptModel> prompts,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await dbContext.Prompt.ExecuteDeleteAsync(cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            foreach (var singlePrompt in prompts)
            {
                await dbContext.Prompt.AddAsync(new()
                {
                    BaseText = singlePrompt.BaseText,
                    PromptName = singlePrompt.PromptName
                }, cancellationToken);
            }
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<PromptModel?> GetPromptCompleteInfoAsync(string promptName,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPromptCompleteInfoAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            var result = await dbContext.Prompt
                .TagWith($"TAG: {nameof(GetPromptCompleteInfoAsync)}")
                .Include(p => p.PromptVariable)
                .Where(p => p.PromptName == promptName)
                .Select(p => new PromptModel()
                {
                    PromptName = promptName,
                    BaseText = p.BaseText,
                    PromptId = p.PromptId
                })
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
