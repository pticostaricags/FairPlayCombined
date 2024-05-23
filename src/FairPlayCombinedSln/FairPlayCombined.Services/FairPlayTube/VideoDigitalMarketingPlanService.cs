using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.FairPlayTube;
using Microsoft.EntityFrameworkCore;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class VideoDigitalMarketingPlanService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory) 
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
    }
}
