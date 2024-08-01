using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.ImageStyle;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreateImageStyleModel,
        UpdateImageStyleModel,
        ImageStyleModel,
        FairPlayCombinedDbContext,
        ImageStyle,
        PaginationRequest,
        PaginationOfT<ImageStyleModel>
        >]
    public partial class ImageStyleService  : BaseService, IImageStyleService
    {
    }
}
