using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Models.Common.Localization;
using FairPlayCombined.Models.Common.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FairPlayTube.Extensions
{
    public static class LocalizationMinimalApisExtensions
    {
        public static WebApplication AddLocalizationEndpoints(this WebApplication app)
        {
            app.MapGet("localization/GetSupportedCultures",
                async ([FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
                CancellationToken cancellationToken) =>
                {
                    var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                    var result = await dbContext.Culture
                                .Select(p => new CultureModel()
                                {
                                    Name = p.Name,
                                    DisplayName = CultureInfo.GetCultureInfo(p.Name).DisplayName
                                })
                                .ToArrayAsync(cancellationToken: cancellationToken);
                    return result;
                });
            app.MapGet("/localization/GetAllResources",
                async ([FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
                CancellationToken cancellationToken) =>
                {
                    var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                    var currentCulture = CultureInfo.CurrentCulture;
                    var result = await dbContext.Resource
                        .Include(p => p.Culture)
                        .Where(p => p.Culture.Name == currentCulture.Name)
                        .AsNoTracking()
                        .Select(p => new ResourceModel()
                        {
                            CultureId = p.CultureId,
                            CultureName = p.Culture.Name,
                            Key = p.Key,
                            ResourceId = p.ResourceId,
                            Type = p.Type,
                            Value = p.Value
                        })
                        .ToArrayAsync(cancellationToken);
                    if (result.Length == 0)
                        result = await dbContext.Resource
                        .Include(p => p.Culture)
                        .Where(p => p.Culture.Name == "en-US")
                        .AsNoTracking()
                        .Select(p => new ResourceModel()
                        {
                            CultureId = p.CultureId,
                            CultureName = p.Culture.Name,
                            Key = p.Key,
                            ResourceId = p.ResourceId,
                            Type = p.Type,
                            Value = p.Value
                        })
                        .ToArrayAsync(cancellationToken);
                    return result;
                });
            return app;
        }
    }
}
