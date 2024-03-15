using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.Common.Resource;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreateResourceModel,
        UpdateResourceModel,
        ResourceModel,
        FairPlayCombinedDbContext,
        Resource,
        PaginationRequest,
        PaginationOfT<ResourceModel>
        >]
    public partial class ResourceService : BaseService
    {
        public async Task<ResourceModel[]> GetAllResourceByCulturIdAsync(int cultureId,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.Resource
                .Where(p=>p.CultureId == cultureId)
            .AsNoTracking()
            .Select(p => new ResourceModel()
            {
                ResourceId = p.ResourceId,
                Type = p.Type,
                Key = p.Key,
                Value = p.Value,
                CultureId = p.CultureId,

            }).ToArrayAsync(cancellationToken);
            return result;
        }
    }
}
