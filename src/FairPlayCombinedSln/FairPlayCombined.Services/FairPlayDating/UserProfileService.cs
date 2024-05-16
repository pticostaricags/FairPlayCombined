using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.UserProfile;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateUserProfileModel,
        UpdateUserProfileModel,
        UserProfileModel,
        FairPlayCombinedDbContext,
        UserProfile,
        PaginationRequest,
        PaginationOfT<UserProfileModel>
        >]
    public partial class UserProfileService : BaseService
    {
        public async Task<long?> GetUserProfileIdByUserIdAsync(string userId,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.UserProfile
                .AsNoTracking()
                .Where(p => p.ApplicationUserId == userId)
                .Select(p => p.UserProfileId)
                .SingleOrDefaultAsync(cancellationToken);
            return result;
        }

        public async Task<UserProfileModel?> GetUserProfileByUserIdAsync(string userId,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.UserProfile
                .AsNoTracking()
                .Where(p => p.ApplicationUserId == userId)
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
                    UserProfileId = p.UserProfileId,
                    MainProfessionId = p.MainProfessionId
                })
                .SingleOrDefaultAsync(cancellationToken);
            return result;
        }
    }
}
