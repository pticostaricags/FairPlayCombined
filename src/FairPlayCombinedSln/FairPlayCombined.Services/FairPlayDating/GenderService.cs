using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Gender;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateGenderModel,
        UpdateGenderModel,
        GenderModel,
        FairPlayCombinedDbContext,
        Gender,
        PaginationRequest,
        PaginationOfT<GenderModel>
        >]
    public partial class GenderService : BaseService
    {
    }
}
