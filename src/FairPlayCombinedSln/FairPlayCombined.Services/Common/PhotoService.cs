using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.Common.Photo;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreatePhotoModel,
        UpdatePhotoModel,
        PhotoModel,
        FairPlayCombinedDbContext,
        Photo,
        PaginationRequest,
        PaginationOfT<PhotoModel>
        >]
    public partial class PhotoService : BaseService
    {
    }
}
