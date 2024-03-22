using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class VideoDigitalMarketingDailyPostsService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory) : BaseService
    {
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
