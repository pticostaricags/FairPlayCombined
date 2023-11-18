using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using FairPlayCombined.Common;

namespace FairPlayCombined.Services.Common
{
    public class RoleService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory) :
        BaseService,
        IRoleService
    {
        public async Task<PaginationOfT<RoleModel>> GetPaginatedRoleListAsync(
            PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            PaginationOfT<RoleModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {base.GetSortTypeString(p.SortType)}"));
            var query = dbContext.AspNetRoles
                .AsNoTracking()
                .Select(p => new RoleModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                });
            if (!String.IsNullOrEmpty(orderByString))
                query = query.OrderBy(orderByString);
            result.TotalItems = await query.CountAsync(cancellationToken: cancellationToken);
            result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems /
                Constants.Pagination.PageSize);
            result.PageSize = Constants.Pagination.PageSize;
            result.Items = await query.Skip(paginationRequest.StartIndex).Take(Constants.Pagination.PageSize)
                .ToArrayAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
