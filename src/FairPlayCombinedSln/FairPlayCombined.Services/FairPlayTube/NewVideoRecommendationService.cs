using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateNewVideoRecommendationModel,
        UpdateNewVideoRecommendationModel,
        NewVideoRecommendationModel,
        FairPlayCombinedDbContext,
        NewVideoRecommendation,
        PaginationRequest,
        PaginationOfT<NewVideoRecommendationModel>
        >]
    public partial class NewVideoRecommendationService : BaseService, INewVideoRecommendationService
    {
    }
}
