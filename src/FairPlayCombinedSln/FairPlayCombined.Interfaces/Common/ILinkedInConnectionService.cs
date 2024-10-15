using FairPlayCombined.Models.Common.LinkedInConnection;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Interfaces.Common
{
    public interface ILinkedInConnectionService
    {
        Task ImportFromConnectionsFileAsync(Stream stream, CancellationToken cancellationToken);
        Task<long> CreateLinkedInConnectionAsync(CreateLinkedInConnectionModel createModel,
            CancellationToken cancellationToken);
        Task<LinkedInConnectionModel[]> GetAllLinkedInConnectionAsync(CancellationToken cancellationToken);
        Task<LinkedInConnectionModel> GetLinkedInConnectionByIdAsync(long id, 
            CancellationToken cancellationToken);
        Task DeleteLinkedInConnectionByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<LinkedInConnectionModel>> GetPaginatedLinkedInConnectionAsync(
        PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
