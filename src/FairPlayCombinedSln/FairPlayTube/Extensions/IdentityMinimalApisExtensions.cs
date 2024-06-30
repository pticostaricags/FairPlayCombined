using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FairPlayTube.Extensions
{
    public static class IdentityMinimalApisExtensions
    {
        public static WebApplication AddCustomIdentityEndpoints(this WebApplication app,
            string clientAppsAuthPolicy)
        {
            app.MapGet("/identity/GetMyRoles",
                async (
                    [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
                    [FromServices] IUserProviderService userProviderService,
                    CancellationToken cancellationToken) =>
            {
                var userId = userProviderService.GetCurrentUserId();
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var roles = await dbContext.AspNetUsers.Where(p => p.Id == userId)
                .SelectMany(p => p.Role)
                .ToArrayAsync(cancellationToken);
                var result = roles.Select(p => p.Name).ToArray();
                return result;
            }).RequireAuthorization(policyNames: clientAppsAuthPolicy);
            return app;
        }
    }
}
