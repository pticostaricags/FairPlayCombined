using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.UserFundsUniqueCodes;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.Common
{
    public class UserFundsUniqueCodesService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService) :
        BaseService,
        IUserFundsUniqueCodesService
    {
        public async Task ClaimFundsUniqueCodeAsync(Guid code, CancellationToken cancellationToken)
        {
            var currentUserId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var userEntity = await dbContext.AspNetUsers
                .Include(p => p.UserFunds)
                .SingleAsync(p => p.Id == currentUserId,
                cancellationToken);
            var uniqueCodeEntity = await dbContext.UserFundsUniqueCodes
                .SingleAsync(p => p.Code.ToString() == code.ToString(),
                cancellationToken);
            if (uniqueCodeEntity.IsClaimed)
                throw new RuleException("Code is already claimed");
            uniqueCodeEntity.IsClaimed = true;
            uniqueCodeEntity.ClaimedByApplicationUserId = currentUserId;
            userEntity.UserFunds.AvailableFunds += 10;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Guid> CreateFundsUniqueCodeAsync(CreateUserFundsUniqueCodesModel createUserFundsUniqueCodesModel, CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            UserFundsUniqueCodes entity = new()
            {
                Code = Guid.NewGuid(),
                IsClaimed = false,
                OwnerFullName = createUserFundsUniqueCodesModel.OwnerFullName,
                OwnerEmailAddress = createUserFundsUniqueCodesModel.OwnerEmailAddress,
                OwnerLinkedProfileUrl = createUserFundsUniqueCodesModel.OwnerLinkedProfileUrl
            };
            await dbContext.UserFundsUniqueCodes.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity.Code;
        }

        public async Task<PaginationOfT<UserFundsUniqueCodesModel>> GetPaginatedUserFundsUniqueCodesListAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            PaginationOfT<UserFundsUniqueCodesModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.UserFundsUniqueCodes
                .AsNoTracking()
                .OrderByDescending(p => p.UserFundsUniqueCodesId)
                .Select(p => new UserFundsUniqueCodesModel()
                {
                    Code = p.Code,
                    IsClaimed = p.IsClaimed,
                    UserFundsUniqueCodesId = p.UserFundsUniqueCodesId,
                    OwnerFullName = p.OwnerFullName,
                    OwnerEmailAddress = p.OwnerEmailAddress,
                    OwnerLinkedProfileUrl = p.OwnerLinkedProfileUrl,
                    ClaimedByApplicationUser = p.ClaimedByApplicationUser.Email
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
