using FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface INewVideoRecommendationService
    {
        Task<long> CreateNewVideoRecommendationAsync(
            CreateNewVideoRecommendationModel createModel,
            CancellationToken cancellationToken);
        Task<NewVideoRecommendationModel[]> GetAllNewVideoRecommendationAsync(
        CancellationToken cancellationToken);
        Task<NewVideoRecommendationModel> GetNewVideoRecommendationByIdAsync(
        long id, CancellationToken cancellationToken);
        Task DeleteNewVideoRecommendationByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<NewVideoRecommendationModel>> GetPaginatedNewVideoRecommendationAsync(
        PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<PaginationOfT<NewVideoRecommendationModel>> GetPaginatedNewVideoRecommendationForUserIdAsync(
        PaginationRequest paginationRequest, string userId, CancellationToken cancellationToken);

        Task<string> GenerateNewVideoRecommendationAsync(string languageCode,
            CancellationToken cancellationToken);
    }
}
