using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.Common.Promp;
using FairPlayCombined.Models.Common.PromptVariable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    public class PromptGeneratorService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<PromptGeneratorService> logger)
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
        public async Task<PromptModel?> GetPromptCompleteInfoAsync(string promptName,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPromptCompleteInfoAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            var result = await dbContext.Prompt.Include(p=>p.PromptVariable)
                .Where(p=>p.PromptName == promptName)
                .Select(p=>new PromptModel()
                {
                    PromptName = promptName,
                    BaseText = p.BaseText,
                    PromptId = p.PromptId,
                    PromptVariables = p.PromptVariable
                    .Select(v=> new PromptVariableModel()
                    {
                        PromptId=v.PromptId,
                        VariableName = v.VariableName
                    }).ToArray()
                })
                .SingleOrDefaultAsync(cancellationToken:cancellationToken);
            return result;
        }
    }
}
