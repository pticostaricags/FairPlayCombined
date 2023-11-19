using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Frequency;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
