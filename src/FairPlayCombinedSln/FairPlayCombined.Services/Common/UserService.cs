using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.Common
{
    public class UserService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory) : 
        BaseService,
        IUserService
    {
        public async Task<PaginationOfT<UserModel>> GetPaginatedUserListAsync(
            PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            PaginationOfT<UserModel> result = new PaginationOfT<UserModel>();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {base.GetSortTypeString(p.SortType)}"));
            var query = dbContext.AspNetUsers
                .AsNoTracking()
                .Select(p => new UserModel()
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    LockoutEnabled = p.LockoutEnabled
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
