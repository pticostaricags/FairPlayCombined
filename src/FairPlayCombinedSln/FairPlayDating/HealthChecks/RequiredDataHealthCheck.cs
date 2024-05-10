using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FairPlayDating.HealthChecks
{
    public class RequiredDataHealthCheck(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<RequiredDataHealthCheck> logger) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Running health checks for: {HealthCheckName}",
                    nameof(RequiredDataHealthCheck));
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var hasRoles = await dbContext.AspNetRoles.AnyAsync(cancellationToken);
                if (!hasRoles)
                {
                    return HealthCheckResult.Unhealthy(description: "No roles found in the application");
                }
                var hasRequiredGenders = await dbContext.Gender.CountAsync(cancellationToken) == 2;
                if (!hasRequiredGenders)
                {
                    return HealthCheckResult.Unhealthy(description: "Required genders missing");
                }
                var hasDateObjectives = await dbContext.DateObjective.AnyAsync(cancellationToken);
                if (!hasDateObjectives)
                {
                    return HealthCheckResult.Unhealthy(description: "Date objectives missing");
                }
                var hasEyesColors = await dbContext.EyesColor.AnyAsync(cancellationToken);
                if (!hasEyesColors)
                {
                    return HealthCheckResult.Unhealthy(description: "Eyes colors missing");
                }
                var hasHairColors = await dbContext.HairColor.AnyAsync(cancellationToken);
                if (!hasHairColors)
                {
                    return HealthCheckResult.Unhealthy(description: "Hair colors missing");
                }
                var hasKidsStatuses = await dbContext.KidStatus.AnyAsync(cancellationToken);
                if (!hasKidsStatuses)
                {
                    return HealthCheckResult.Unhealthy(description: "Kids statuses missing");
                }
                var hasReligions = await dbContext.Religion.AnyAsync(cancellationToken);
                if (!hasReligions)
                {
                    return HealthCheckResult.Unhealthy(description: "Religions missing");
                }
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                context.Registration.FailureStatus.ToString(),
                exception: ex);
            }
        }
    }
}
