using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.KidStatus;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateKidStatusModel,
        UpdateKidStatusModel,
        KidStatusModel,
        FairPlayCombinedDbContext,
        KidStatus,
        PaginationRequest,
        PaginationOfT<KidStatusModel>
        >]
    public partial class KidStatusService : BaseService
    {
    }
}
