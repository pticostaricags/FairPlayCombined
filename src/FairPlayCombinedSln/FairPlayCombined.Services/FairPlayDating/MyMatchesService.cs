using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayDating.UserProfile;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.FairPlayDating
{
    public class MyMatchesService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService) : BaseService
    {
        public async Task<PaginationOfT<UserProfileModel>?> GetPagedMyPotentialMatchesAsync(
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken)
        {
            PaginationOfT<UserProfileModel>? result = null;
            var myUserId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var myUserProfile = await dbContext.AspNetUsers
                .AsNoTracking()
                .Include(p=>p.UserProfile)
                .Where(p=>p.Id == myUserId)
                .Select(p=>p.UserProfile)
                .SingleOrDefaultAsync();
            if (myUserProfile != null)
            {
                string orderByString = string.Empty;
                if (paginationRequest.SortingItems?.Length > 0)
                    orderByString =
                        String.Join(",",
                        paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
                var query = dbContext.UserProfile
                .Include(p => p.ProfilePhoto)
                .Include(p => p.ApplicationUser).ThenInclude(p => p.LikedUserProfileLikedApplicationUser)
                .Where(p =>
                (Math.Abs(p.BirthDate.Year - myUserProfile.BirthDate.Year) < Constants.Matches.MaxAllowedAgeDifference) &&
                p.BiologicalGenderId != myUserProfile.BiologicalGenderId &&
                p.ApplicationUserId != myUserProfile.ApplicationUserId
                &&
                p.ApplicationUser.LikedUserProfileLikedApplicationUser
                .Any(x => x.LikingApplicationUserId == myUserProfile.ApplicationUserId) == false)
                .Select(p => new UserProfileModel()
                {
                    About = p.About,
                    ApplicationUserId = p.ApplicationUserId,
                    BiologicalGenderId = p.BiologicalGenderId,
                    BirthDate = p.BirthDate,
                    CurrentDateObjectiveId = p.CurrentDateObjectiveId,
                    CurrentLatitude = p.CurrentLatitude,
                    CurrentLongitude = p.CurrentLongitude,
                    EyesColorId = p.EyesColorId,
                    HairColorId = p.HairColorId,
                    KidStatusId = p.KidStatusId,
                    PreferredEyesColorId = p.PreferredEyesColorId,
                    PreferredHairColorId = p.PreferredHairColorId,
                    PreferredKidStatusId = p.PreferredKidStatusId,
                    PreferredReligionId = p.PreferredReligionId,
                    PreferredTattooStatusId = p.PreferredTattooStatusId,
                    ProfilePhotoId = p.ProfilePhotoId,
                    ReligionId = p.ReligionId,
                    TattooStatusId = p.TattooStatusId,
                    UserProfileId = p.UserProfileId
                });
                if (!String.IsNullOrEmpty(orderByString))
                    query = query.OrderBy(orderByString);
                result = new();
                result.PageSize = Constants.Pagination.PageSize;
                result.TotalItems = await query.CountAsync(cancellationToken);
                result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems / result.PageSize);
                result.Items = await query
                    .Skip(paginationRequest.StartIndex)
                    .Take(paginationRequest.PageSize)
                    .ToArrayAsync(cancellationToken);
            }
            return result;
        }
    }
}
