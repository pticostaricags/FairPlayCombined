using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Models.FairPlaySocial.VideoCaptions;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class VideoCaptionsService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory)
    {
        public async Task<string?> GetVideoCaptionsByVideoInfoIdAndLanguageAsync(
            long videoInfoId, string language,CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.VideoCaptions
            .Include(p => p.VideoInfo)
                .Where(p => p.VideoInfoId == videoInfoId &&
                p.Language == language)
                .OrderBy(p => p.Language)
                .Select(p => p.Content)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return result;
        }
        public async Task<VideoCaptionsModel[]> GetVideoCaptionsByVideoInfoIdAsync(
            long videoInfoId, CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.VideoCaptions
            .Include(p => p.VideoInfo)
                .Where(p => p.VideoInfoId == videoInfoId)
                .OrderBy(p => p.Language)
                .Select(p=>new VideoCaptionsModel() 
                {
                    Content = p.Content,
                    Language = p.Language
                })
                .ToArrayAsync(cancellationToken);
            return result;
        }
    }
}
