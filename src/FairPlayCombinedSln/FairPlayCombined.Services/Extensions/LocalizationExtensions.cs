using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Shared.CustomLocalization.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Extensions
{
    public static class LocalizationExtensions
    {
        public static void AddDatabaseDrivenLocalization(this IServiceCollection services)
        {
            services.AddTransient<IStringLocalizerFactory, EFStringLocalizerFactory>();
            services.AddTransient<IStringLocalizer, EFStringLocalizer>();
            services.AddLocalization();
        }
        public async static Task UseDatabaseDrivenLocalization(this WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                using var ctx = scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();
                var supportedCultures = await ctx.Culture.Select(p => p.Name).ToArrayAsync();
                var localizationOptions = new RequestLocalizationOptions()
                    .SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);

                app.UseRequestLocalization(localizationOptions);
            }
            catch (Exception)
            {
                //Ignore
            }
        }
    }
}
