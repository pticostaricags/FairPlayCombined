using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FairPlayTube.HealthChecks
{
    public class FairPlayTubeHealthCheck(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<FairPlayTubeHealthCheck> logger) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Running health checks for: {HealthCheckName}",
                    nameof(FairPlayTubeHealthCheck));
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var roles = await dbContext.AspNetRoles.ToArrayAsync(cancellationToken);
                if (roles?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Missing roles");
                }
                var videoIndexStatuses = await dbContext.VideoIndexStatus.ToArrayAsync(cancellationToken);
                if (videoIndexStatuses?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Missing video index statuses");
                }
                var videoVisibilities = await dbContext.VideoVisibility.ToArrayAsync(cancellationToken);
                if (videoVisibilities?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Missing video visibilities");
                }
                var data = new Dictionary<string, object>()
                {
                    {nameof(roles), roles! },
                    {nameof(videoIndexStatuses), videoIndexStatuses! },
                    {nameof(videoVisibilities), videoVisibilities! }
                };
                return HealthCheckResult.Healthy(data: data.AsReadOnly());
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(context.Registration.FailureStatus.ToString(),
                    exception: ex);
            }
        }
    }
}
