using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.Profession;
using FairPlayCombined.Models.Pagination;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateProfessionModel,
        UpdateProfessionModel,
        ProfessionModel,
        FairPlayCombinedDbContext,
        Profession,
        PaginationRequest,
        PaginationOfT<ProfessionModel>
        >]
    public partial class ProfessionService : BaseService
    {
    }
}
