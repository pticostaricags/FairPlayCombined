using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.Common.Photo;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
