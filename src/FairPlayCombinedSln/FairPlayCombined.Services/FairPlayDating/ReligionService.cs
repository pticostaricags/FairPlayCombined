using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Religion;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateReligionModel,
        UpdateReligionModel,
        ReligionModel,
        FairPlayCombinedDbContext,
        Religion,
        PaginationRequest,
        PaginationOfT<ReligionModel>
        >]
    public partial class ReligionService : BaseService
    {
    }
}
