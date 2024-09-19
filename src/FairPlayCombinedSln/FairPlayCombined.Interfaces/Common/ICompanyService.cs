using FairPlayCombined.Models.Common.Company;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Interfaces.Common;
public interface ICompanyService
{
    Task<long> CreateCompanyAsync(
    CreateCompanyModel createModel,
    CancellationToken cancellationToken
    );
    Task<CompanyModel[]> GetAllCompanyAsync(
    CancellationToken cancellationToken);
    Task<CompanyModel> GetCompanyByIdAsync(
    long id,
    CancellationToken cancellationToken);
    Task DeleteCompanyByIdAsync(
    long id,
    CancellationToken cancellationToken);
    Task<PaginationOfT<CompanyModel>> GetPaginatedCompanyAsync(
    PaginationRequest paginationRequest,
    CancellationToken cancellationToken);
}