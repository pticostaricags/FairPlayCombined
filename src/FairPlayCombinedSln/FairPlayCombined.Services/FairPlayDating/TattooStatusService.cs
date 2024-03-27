using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.TattooStatus;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateTattooStatusModel,
        UpdateTattooStatusModel,
        TattooStatusModel,
        FairPlayCombinedDbContext,
        TattooStatus,
        PaginationRequest,
        PaginationOfT<TattooStatusModel>
        >]
    public partial class TattooStatusService : BaseService
    {
    }
}
