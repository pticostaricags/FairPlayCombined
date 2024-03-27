using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Frequency;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateFrequencyModel,
        UpdateFrequencyModel,
        FrequencyModel,
        FairPlayCombinedDbContext,
        Frequency,
        PaginationRequest,
        PaginationOfT<FrequencyModel>
        >]
    public partial class FrequencyService : BaseService
    {
    }
}
