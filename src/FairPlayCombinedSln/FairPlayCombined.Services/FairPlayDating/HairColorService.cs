using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.HairColor;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateHairColorModel,
        UpdateHairColorModel,
        HairColorModel,
        FairPlayCombinedDbContext,
        HairColor,
        PaginationRequest,
        PaginationOfT<HairColorModel>
        >]
    public partial class HairColorService : BaseService
    {
    }
}
