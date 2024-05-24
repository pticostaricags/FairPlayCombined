using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Interfaces.FairPlayDating;
using FairPlayCombined.Models.FairPlayDating.UserProfile;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
    public partial class UserProfileService : BaseService, IUserProfileService
    {
        public async Task<long> CreateUserProfileExtendedAsync(
    CreateUserProfileModel createModel,
    CancellationToken cancellationToken
    )
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(CreateUserProfileAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            UserProfile entity = new()
            {
                ApplicationUserId = createModel.ApplicationUserId,
                About = createModel.About,
                HairColorId = createModel.HairColorId,
                PreferredHairColorId = createModel.PreferredHairColorId,
                EyesColorId = createModel.EyesColorId,
                PreferredEyesColorId = createModel.PreferredEyesColorId,
                BiologicalGenderId = createModel.BiologicalGenderId,
                CurrentDateObjectiveId = createModel.CurrentDateObjectiveId,
                ReligionId = createModel.ReligionId,
                PreferredReligionId = createModel.PreferredReligionId,
                CurrentLatitude = createModel.CurrentLatitude,
                CurrentLongitude = createModel.CurrentLongitude,
                ProfilePhotoId = createModel.ProfilePhotoId,
                KidStatusId = createModel.KidStatusId,
                PreferredKidStatusId = createModel.PreferredKidStatusId,
                TattooStatusId = createModel.TattooStatusId,
                PreferredTattooStatusId = createModel.PreferredTattooStatusId,
                MainProfessionId = createModel.MainProfessionId,
                BirthDate = createModel.BirthDate,
                CurrentGeoLocation = createModel.CurrentGeoLocation,

            };
            await dbContext.UserProfile.AddAsync(entity, cancellationToken);
            if (createModel.ActivitiesFrequency?.Count > 0)
            {
                var userEntity = await dbContext.AspNetUsers.SingleAsync(p => p.Id == createModel.ApplicationUserId,
                    cancellationToken);
                await dbContext.UserActivity.Where(p => p.ApplicationUserId == createModel.ApplicationUserId)
                    .ExecuteDeleteAsync(cancellationToken);
                foreach (var singleUserActivity in createModel.ActivitiesFrequency)
                {
                    userEntity.UserActivity.Add(new UserActivity()
                    {
                        ActivityId = singleUserActivity.ActivityId,
                        FrequencyId = singleUserActivity.FrequencyId
                    });
                }
            }
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity.UserProfileId;
        }
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
                    MainProfessionText = p.MainProfession.Name,
                    PreferredEyesColorId = p.PreferredEyesColorId,
                    PreferredHairColorId = p.PreferredHairColorId,
                    PreferredKidStatusId = p.PreferredKidStatusId,
                    PreferredReligionId = p.PreferredReligionId,
                    PreferredTattooStatusId = p.PreferredTattooStatusId,
                    ProfilePhotoId = p.ProfilePhotoId,
                    ReligionId = p.ReligionId,
                    TattooStatusId = p.TattooStatusId,
                    UserProfileId = p.UserProfileId,
                    MainProfessionId = p.MainProfessionId,
                    ActivitiesFrequency = p.ApplicationUser.UserActivity.Select(x => new UserProfileActivityFrequencyModel()
                    {
                        FrequencyId = x.FrequencyId,
                        ActivityId = x.ActivityId
                    }).ToArray()
                })
                .SingleOrDefaultAsync(cancellationToken);
            return result;
        }
    }
}
