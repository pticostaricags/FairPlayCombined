using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.Common.Resource;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
        public async Task<ResourceModel[]> GetAllResourceSortedAsync(CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.Resource
                .OrderBy(p => p.Key).ThenBy(p => p.Type)
                .ThenBy(p => p.CultureId)
            .AsNoTracking()
            .Select(p => new ResourceModel()
            {
                CultureName = p.Culture.Name,
                ResourceId = p.ResourceId,
                Type = p.Type,
                Key = p.Key,
                Value = p.Value,
                CultureId = p.CultureId,

            }).ToArrayAsync(cancellationToken);
            return result;
        }

        public async Task UpdateResourcesAsync(ResourceModel[] resources, CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var existentResourceIds = resources.Where(p => p.ResourceId != 0)
                .Select(p => p.ResourceId);
            foreach (var singleExistentResource in dbContext.Resource.Where(p => existentResourceIds.Contains(p.ResourceId)))
            {
                var updatedModel = resources.Single(p => p.ResourceId == singleExistentResource.ResourceId);
                singleExistentResource.Value = updatedModel.Value;
            }
            foreach (var singleNewResource in resources.Where(p => p.ResourceId == 0))
            {
                Resource resource = new Resource()
                {
                    CultureId = singleNewResource.CultureId,
                    Key = singleNewResource.Key,
                    Type = singleNewResource.Type,
                    Value = singleNewResource.Value
                };
                await dbContext.Resource.AddAsync(resource, cancellationToken: cancellationToken);
            }
            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
            }
        }
    }
}