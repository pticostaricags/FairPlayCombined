using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Interceptors.Interfaces;
using FairPlayCombined.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.DataAccess.Interceptors
{
    public class SaveChangesInterceptor(IUserProviderService userProviderService) : ISaveChangesInterceptor
    {
        public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            var changedEntities = eventData!.Context!.ChangeTracker.Entries();
            foreach (var entityEntry in changedEntities.Where(entityEntry => entityEntry.Entity is IOriginatorInfo))
            {
                if (entityEntry.Entity is IOriginatorInfo entity)
                {
                    var connectionString = eventData.Context.Database.GetConnectionString();
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new(connectionString);
                    var applicationName = sqlConnectionStringBuilder.ApplicationName ?? "Unknown App";
                    var userName = userProviderService.GetCurrentUserId() ?? "Unauthenticated User";
                    if (entityEntry.State == EntityState.Added)
                    {
                        entity.SourceApplication = applicationName;
                        entity.RowCreationDateTime = DateTimeOffset.UtcNow;
                        entity.RowCreationUser = userName!;
                        entity.OriginatorIpaddress = String.Join(",", await IpAddressProvider.GetCurrentHostIPv4AddressesAsync());
                    }
                }
            }

            return result;
        }

        public InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            return result;
        }
    }
}
