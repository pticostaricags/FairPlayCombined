using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.ObjectModel;

namespace FairPlayDating.HealthChecks
{
    public class FairPlayDatingHealthCheck(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<FairPlayDatingHealthCheck> logger) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Running health checks for: {HealthCheckName}",
                    nameof(FairPlayDatingHealthCheck));
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var roles = await dbContext.AspNetRoles.ToArrayAsync(cancellationToken);
                if (roles?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "No roles found in the application");
                }
                var activities = await dbContext.Activity.ToArrayAsync(cancellationToken);
                if (activities?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Activities missing");
                }
                var frequencies = await dbContext.Frequency.ToArrayAsync(cancellationToken);
                if (frequencies?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Frequencies missing");
                }
                var requiredGenders = await dbContext.Gender.ToArrayAsync(cancellationToken);
                if (requiredGenders?.Length < 2)
                {
                    return HealthCheckResult.Unhealthy(description: "Required genders missing");
                }
                var dateObjectives = await dbContext.DateObjective.ToArrayAsync(cancellationToken);
                if (dateObjectives?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Date objectives missing");
                }
                var eyesColors = await dbContext.EyesColor.ToArrayAsync(cancellationToken);
                if (eyesColors?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Eyes colors missing");
                }
                var hairColors = await dbContext.HairColor.ToArrayAsync(cancellationToken);
                if (hairColors?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Hair colors missing");
                }
                var kidsStatuses = await dbContext.KidStatus.ToArrayAsync(cancellationToken);
                if (kidsStatuses?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Kids statuses missing");
                }
                var religions = await dbContext.Religion.ToArrayAsync(cancellationToken);
                if (religions?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Religions missing");
                }
                var tatooStatuses = await dbContext.TattooStatus.ToArrayAsync(cancellationToken);
                if (tatooStatuses?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Tattoo statuses missing");
                }
                var professions = await dbContext.Profession.ToArrayAsync(cancellationToken);
                if (professions?.Length == 0)
                {
                    return HealthCheckResult.Unhealthy(description: "Professions missing");
                }
                Dictionary<string, object> data = new()
                {
                    {nameof(roles), roles! },
                    {nameof(activities), activities! },
                    {nameof(frequencies), frequencies! },
                    {nameof(requiredGenders), requiredGenders! },
                    {nameof(dateObjectives), dateObjectives! },
                    {nameof(eyesColors), eyesColors! },
                    {nameof(hairColors), hairColors! },
                    {nameof(kidsStatuses), kidsStatuses! },
                    {nameof(religions), religions! },
                    {nameof(tatooStatuses), tatooStatuses! },
                    {nameof(professions), professions!}
                };
                return HealthCheckResult.Healthy(data: data.AsReadOnly());
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
