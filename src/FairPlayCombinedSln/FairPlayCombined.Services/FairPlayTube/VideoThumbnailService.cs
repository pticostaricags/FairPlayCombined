using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Models.FairPlayTube.VideoThumbnail;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
    CreateVideoThumbnailModel,
    UpdateVideoThumbnailModel,
    VideoThumbnailModel,
    FairPlayCombinedDbContext,
    VideoThumbnail,
    PaginationRequest,
    PaginationOfT<VideoThumbnailModel>
    >]
    public partial class VideoThumbnailService : BaseService
    {
    }
}
