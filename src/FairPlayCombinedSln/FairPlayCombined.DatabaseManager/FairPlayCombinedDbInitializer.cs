using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DatabaseManager.Properties;
using Microsoft.EntityFrameworkCore;

namespace FairPlayCombined.DatabaseManager;

public class FairPlayCombinedDbInitializer(ILogger<FairPlayCombinedDbInitializer> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Start of method: {MethodName}", nameof(ExecuteAsync));
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(dbContext.Database.EnsureCreatedAsync, stoppingToken);
        await strategy.ExecuteInTransactionAsync(
            operation: async (cancellationToken) =>
            {

                await dbContext.Database.ExecuteSqlRawAsync(Resources._1_Script_PostDeployment1,
                    cancellationToken);
                await dbContext.Database.ExecuteSqlRawAsync(Resources._2_FairPlayDating,
                    cancellationToken);
                await dbContext.Database.ExecuteSqlRawAsync(Resources._3_FairPlaySocial,
                    cancellationToken);
                await dbContext.Database.ExecuteSqlRawAsync(Resources._4_FairPlayTube,
                    cancellationToken);
                await dbContext.Database.ExecuteSqlRawAsync(Resources._5_FairPlayBudget,
                    cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            },
            verifySucceeded: async (cancellationToken) =>
            {
                var rolesCount = await dbContext.AspNetRoles.CountAsync(cancellationToken);
                var currenciesCount = await dbContext.Currency.CountAsync(cancellationToken);
                return rolesCount > 0 && currenciesCount == 2;
            },
            stoppingToken
            );
    }
}
