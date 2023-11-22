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
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using System.Linq.Expressions;

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
                .Include(p => p.UserProfile)
                .Where(p => p.Id == myUserId)
                .Select(p => p.UserProfile)
                .SingleOrDefaultAsync();
            if (myUserProfile != null)
            {
                var query = dbContext.UserProfile
                .Include(p => p.ProfilePhoto)
                .Include(p => p.ApplicationUser).ThenInclude(p => p.LikedUserProfileLikedApplicationUser)
                .Where(p =>
                (Math.Abs(p.BirthDate.Year - myUserProfile.BirthDate.Year) < Constants.Matches.MaxAllowedAgeDifference) &&
                p.BiologicalGenderId != myUserProfile.BiologicalGenderId &&
                p.ApplicationUserId != myUserProfile.ApplicationUserId
                &&
                p.ApplicationUser.LikedUserProfileLikedApplicationUser
                .Any(x => x.LikingApplicationUserId == myUserProfile.ApplicationUserId) == false);
                result = new();
                result.PageSize = paginationRequest.PageSize;
                result.TotalItems = await query.CountAsync(cancellationToken);
                result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems / result.PageSize);
                result.Items = await query
                    .OrderByDescending(GetSortByBetterMatchExpression(myUserProfile))
                    .Skip(paginationRequest.StartIndex)
                    .Take(paginationRequest.PageSize)
                .Select(p => new UserProfileModel()
                {
                    Age = EF.Functions.DateDiffYear(p.BirthDate, DateTimeOffset.UtcNow),
                    About = p.About,
                    ApplicationUserId = p.ApplicationUserId,
                    BiologicalGenderText = p.BiologicalGender.Name,
                    BirthDate = p.BirthDate,
                    CurrentDateObjectiveText = p.CurrentDateObjective.Name,
                    CurrentLatitude = p.CurrentLatitude,
                    CurrentLongitude = p.CurrentLongitude,
                    EyesColorText = p.EyesColor.Name,
                    HairColorText = p.HairColor.Name,
                    KidStatusText = p.KidStatus.Name,
                    PreferredEyesColorText = p.PreferredEyesColor.Name,
                    PreferredHairColorText = p.PreferredHairColor.Name,
                    PreferredKidStatusText = p.PreferredKidStatus.Name,
                    PreferredReligionText = p.PreferredReligion.Name,
                    PreferredTattooStatusText = p.PreferredTattooStatus.Name,
                    ProfilePhotoId = p.ProfilePhotoId,
                    ReligionText = p.Religion.Name,
                    TattooStatusText = p.TattooStatus.Name,
                    UserProfileId = p.UserProfileId,
                    Distance = p.CurrentGeoLocation!.Distance(myUserProfile.CurrentGeoLocation)
                })
                .ToArrayAsync(cancellationToken);
            }
            return result;
        }

        private static Expression<Func<UserProfile, double>> GetSortByBetterMatchExpression(UserProfile myUserProfile)
        {
            return p =>
            (-1 * myUserProfile.CurrentGeoLocation!.Distance(p.CurrentGeoLocation!)) +
                            (myUserProfile.CurrentDateObjectiveId == p.CurrentDateObjectiveId ? 1 : 0) +
                            (myUserProfile.PreferredEyesColorId == p.EyesColorId ? 1 : 0);
        }
    }
}
