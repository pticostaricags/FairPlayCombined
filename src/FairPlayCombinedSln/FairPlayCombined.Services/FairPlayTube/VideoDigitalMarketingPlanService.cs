﻿using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class VideoDigitalMarketingPlanService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory) : BaseService
    {
        public async Task<string?> GetVideoDigitalMarketingPlanAsync(
            long videoInfoId,
            string socialNetworkName,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken:cancellationToken);
            var result = await dbContext.VideoDigitalMarketingPlan.Where(
                p => p.VideoInfoId == videoInfoId && p.SocialNetworkName == socialNetworkName)
                .Select(p => p.HtmlDigitalMarketingPlan)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return result;
        }
        public async Task SaveVideoDigitalMarketingPlanAsync(long videoInfoId, 
            string htmlDigitalMarketingPlan,
            string socialNetworkName,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            await dbContext.VideoDigitalMarketingPlan.AddAsync(new() 
            {
                VideoInfoId = videoInfoId,
                HtmlDigitalMarketingPlan= htmlDigitalMarketingPlan,
                SocialNetworkName= socialNetworkName
            },
                cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }
    }
}
