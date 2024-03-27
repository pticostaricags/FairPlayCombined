using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Gender;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateGenderModel,
        UpdateGenderModel,
        GenderModel,
        FairPlayCombinedDbContext,
        Gender,
        PaginationRequest,
        PaginationOfT<GenderModel>
        >]
    public partial class GenderService : BaseService
    {
    }
}
