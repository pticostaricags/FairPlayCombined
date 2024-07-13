﻿using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class FairPlayTubeBillingService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IStringLocalizer<FairPlayTubeBillingService> localizer) : IFairPlayTubeBillingService
    {
        public async Task<FairPlayTubeBillingModel[]> GetBillingInfoByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            string creatingThumbnailForVideoText = localizer[CreatingThumbnailForVideoTextKey];
            string indexingOfVideoText = localizer[IndexingOfVideoTextKey];

            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.VideoThumbnail
                .AsNoTracking()
                .Where(p => p.VideoInfo.ApplicationUserId == userId)
                .Select(p => new FairPlayTubeBillingModel()
                {
                    Details = String.Concat(creatingThumbnailForVideoText, p.VideoInfo.Name),
                    RowCreationDateTime = p.OpenAiprompt.RowCreationDateTime,
                    OperationCost = p.OpenAiprompt.OperationCost
                })
                .Union(
                dbContext.VideoIndexingTransaction
                .AsNoTracking()
                .Where(p => p.VideoInfo.ApplicationUserId == userId)
                .Select(p => new FairPlayTubeBillingModel()
                {
                    Details = String.Concat(indexingOfVideoText, p.VideoInfo.Name),
                    OperationCost =p.IndexingCost,
                    RowCreationDateTime = p.RowCreationDateTime
                })
                .OrderByDescending(p => p.RowCreationDateTime)
                ).ToArrayAsync(cancellationToken);
            return result;
        }

        [ResourceKey("Creating thumbnail for video: ")]
        public const string CreatingThumbnailForVideoTextKey = "CreatingThumbnailForVideoText";
        [ResourceKey(defaultValue: "Indexing Of Video: ")]
        public const string IndexingOfVideoTextKey = "IndexingOfVideoText";
    }
}
