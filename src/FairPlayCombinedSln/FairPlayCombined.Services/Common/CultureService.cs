using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FairPlayCombined.Services.Common
{
    public class CultureService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory) :
        ICultureService
    {
        public async Task<string[]> GetSupportedCultures(CancellationToken cancellationToken)
        {
            using var fairPlayShopDatabaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            var result = await fairPlayShopDatabaseContext.Culture.AsNoTracking()
                .Select(c => c.Name)
                .ToArrayAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
