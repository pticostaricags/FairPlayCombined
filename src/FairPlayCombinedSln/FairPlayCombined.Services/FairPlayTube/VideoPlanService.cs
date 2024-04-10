using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Models.FairPlayTube.VideoPlan;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateVideoPlanModel,
        UpdateVideoPlanModel,
        VideoPlanModel,
        FairPlayCombinedDbContext,
        VideoPlan,
        PaginationRequest,
        PaginationOfT<VideoPlanModel>
        >]
    public partial class VideoPlanService : BaseService
    {
    }
}
