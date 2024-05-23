using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.AzureVideoIndexer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    public partial class SupportedLanguageService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<SupportedLanguageService> logger) : BaseService, ISupportedLanguageService
    {
        public async Task<SupportedLanguageModel[]?> GetAllSupportedLanguageForVideoInfoIdAsync(
            long videoInfoId,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetAllSupportedLanguageAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var laguagesWithCaptions = await dbContext.VideoCaptions
                .Where(p => p.VideoInfoId == videoInfoId)
                .Select(p => p.Language).ToArrayAsync(cancellationToken);
            var result = await dbContext.VideoIndexerSupportedLanguage
                .Where(p=> laguagesWithCaptions.Contains(p.LanguageCode))
                .OrderBy(p=>p.Name)
                .Select(p => new SupportedLanguageModel
                {
                    name = p.Name,
                    languageCode = p.LanguageCode
                })
                .ToArrayAsync(cancellationToken: cancellationToken);
            return result;
        }

        public async Task<SupportedLanguageModel[]?> GetAllSupportedLanguageAsync(
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetAllSupportedLanguageAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.VideoIndexerSupportedLanguage
                .Select(p => new SupportedLanguageModel 
                {
                    name = p.Name,
                    languageCode = p.LanguageCode
                })
                .ToArrayAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
