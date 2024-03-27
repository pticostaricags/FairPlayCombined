using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.EyesColor;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateEyesColorModel,
        UpdateEyesColorModel,
        EyesColorModel,
        FairPlayCombinedDbContext,
        EyesColor,
        PaginationRequest,
        PaginationOfT<EyesColorModel>
        >]
    public partial class EyesColorService : BaseService
    {
    }
}
