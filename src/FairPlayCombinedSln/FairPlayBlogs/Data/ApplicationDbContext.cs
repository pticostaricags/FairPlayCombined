using FairPlayCombined.Common;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;

namespace FairPlayBlogs.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
    {
        if (entity is ApplicationUser entry)
        {
            var applicationName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name;
            entry!.SourceApplication = applicationName;
            entry.RowCreationDateTime = DateTimeOffset.UtcNow;
            entry.RowCreationUser = entry.UserName!;
            entry.OriginatorIpaddress = String.Join(",", IpAddressProvider.GetCurrentHostIPv4AddressesAsync().Result);
        }
        return base.Add(entity);
    }

    public override async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is ApplicationUser entry)
        {
            var applicationName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name;
            entry!.SourceApplication = applicationName;
            entry.RowCreationDateTime = DateTimeOffset.UtcNow;
            entry.RowCreationUser = entry.UserName!;
            entry.OriginatorIpaddress = String.Join(",", await IpAddressProvider.GetCurrentHostIPv4AddressesAsync());
        }
        return await base.AddAsync(entity, cancellationToken);
    }
}
