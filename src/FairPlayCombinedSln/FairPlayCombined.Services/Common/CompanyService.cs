using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.Company;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreateCompanyModel,
        UpdateCompanyModel,
        CompanyModel,
        FairPlayCombinedDbContext,
        Company,
        PaginationRequest,
        PaginationOfT<CompanyModel>
        >]
    public partial class CompanyService : BaseService, ICompanyService
    {
    }
}
