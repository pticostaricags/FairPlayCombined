using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.DatabaseManager
{
    internal class FairPlayCombinedDbInitializerHealthCheck(FairPlayCombinedDbInitializer 
        dbInitializer) : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            var task = dbInitializer.ExecuteTask;

            return task switch
            {
                { IsCompletedSuccessfully: true } => Task.FromResult(HealthCheckResult.Healthy()),
                { IsFaulted: true } => Task.FromResult(HealthCheckResult.Unhealthy(task.Exception?.InnerException?.Message, task.Exception)),
                { IsCanceled: true } => Task.FromResult(HealthCheckResult.Unhealthy("Database initialization was canceled")),
                _ => Task.FromResult(HealthCheckResult.Degraded("Database initialization is still in progress"))
            };
        }
    }
}
