using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DatabaseManager.Properties;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Threading;

namespace FairPlayCombined.DatabaseManager;

public class FairPlayCombinedDbInitializer(ILogger<FairPlayCombinedDbInitializer> logger, 
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(dbContext.Database.EnsureCreatedAsync, stoppingToken);
        await strategy.ExecuteInTransactionAsync(
            operation: async (cancellationToken) => 
            {
                
                await dbContext.Database.ExecuteSqlRawAsync(Resources._1_Script_PostDeployment1,
                    cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            },
            verifySucceeded: async (cancellationToken) => 
            {
                var rolesCount = await dbContext.AspNetRoles.CountAsync(cancellationToken);
                return rolesCount > 0;
            }
            );
        var roles = await dbContext.AspNetRoles.ToArrayAsync(stoppingToken);
    }
}
