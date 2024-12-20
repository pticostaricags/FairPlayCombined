using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.AzureOpenAI;
using FairPlayCombined.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FairPlayCombined.LocalizationGenerator;

public class LocalizationGenerator(IServiceScopeFactory serviceScopeFactory,
        ILogger<LocalizationGenerator> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopwatch = Stopwatch.StartNew();
        using var scope = serviceScopeFactory.CreateScope();
        var customCache = scope.ServiceProvider.GetRequiredService<ICustomCache>();
        var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var skipTranslations = Convert.ToBoolean(conf["skipTranslations"]);
        FairPlayCombinedDbContext fairPlayCombinedDbContext =
            scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();
        List<Type> typesToCheck = GetTypesToCheck();
        await InsertNewResources(fairPlayCombinedDbContext, typesToCheck, stoppingToken);
        if (skipTranslations)
        {
            logger.LogInformation("Skipping Translation");
            return;
        }
        var allEnglishUSKeys =
            await fairPlayCombinedDbContext.Resource
            .Include(p => p.Culture)
            .Where(p => p.Culture.Name == "en-US")
            .ToListAsync(stoppingToken);
        IAzureOpenAIService azureOpenAIService =
            scope.ServiceProvider.GetRequiredService<IAzureOpenAIService>();
        try
        {
            foreach (var resource in allEnglishUSKeys)
            {
                foreach (var singleCulture in await fairPlayCombinedDbContext.Culture.ToArrayAsync(cancellationToken: stoppingToken))
                {
                    await InsertTranslations(logger, fairPlayCombinedDbContext,
                        azureOpenAIService, resource, singleCulture,
                        customCache, stoppingToken);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
        }
        logger.LogInformation("Process {ProcessName} completed. Time Elapsed: {TimeElapsed}", nameof(LocalizationGenerator), stopwatch.Elapsed);
        stopwatch.Stop();
    }

    private static async Task InsertTranslations(ILogger<LocalizationGenerator> logger,
        FairPlayCombinedDbContext fairPlayCombinedDbContext,
        IAzureOpenAIService azureOpenAIService,
        Resource? resource,
        Culture? singleCulture,
        ICustomCache customCache,
        CancellationToken stoppingToken)
    {
        if (!await fairPlayCombinedDbContext.Resource
                                .AnyAsync(p => p.CultureId == singleCulture!.CultureId &&
                                p.Key == resource!.Key && p.Type == resource.Type, cancellationToken: stoppingToken))
        {
            logger.LogInformation("Translating: \"{Value}\" to \"{Name}\"", resource!.Value, singleCulture!.Name);
            const string sourceLocale = "en-US";
            string cacheKey = $"{resource.Value}-{sourceLocale}-{singleCulture.Name}";
            var translatedText = await customCache.GetOrCreateAsync<string?>(cacheKey,
                retrieveDataTask: async () =>
            {
                TranslationResponse? translationResponse = await
                azureOpenAIService!
                .TranslateSimpleTextAsync(resource.Value,
                sourceLocale, singleCulture.Name,
                cancellationToken: stoppingToken);
                return translationResponse?.TranslatedText;
            },
            TimeSpan.FromDays(1), stoppingToken);
            await fairPlayCombinedDbContext.Resource
                .AddAsync(new Resource()
                {
                    CultureId = singleCulture.CultureId,
                    Key = resource.Key,
                    Type = resource.Type,
                    Value = translatedText ?? resource.Value
                },
                cancellationToken: stoppingToken);
            await fairPlayCombinedDbContext
                .SaveChangesAsync(cancellationToken: stoppingToken);
        }
    }

    private static async Task InsertNewResources(FairPlayCombinedDbContext fairPlayCombinedDbContext, List<Type> typesToCheck, CancellationToken stoppingToken)
    {
        foreach (var singleTypeToCheck in typesToCheck)
        {
            string typeFullName = singleTypeToCheck!.FullName!;
            var fields = singleTypeToCheck.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy
                );
            foreach (var singleField in fields)
            {
                var resourceKeyAttributes =
                    singleField.GetCustomAttributes<ResourceKeyAttribute>();
                if (resourceKeyAttributes != null && resourceKeyAttributes.Any())
                {
                    ResourceKeyAttribute keyAttribute = resourceKeyAttributes.Single();
                    string key = singleField.GetRawConstantValue()!.ToString()!;
                    var entity =
                        await fairPlayCombinedDbContext.Resource
                        .SingleOrDefaultAsync(p => p.CultureId == 1 &&
                        p.Key == key &&
                        p.Type == typeFullName, stoppingToken);
                    if (entity is null)
                    {
                        entity = new Resource()
                        {
                            CultureId = 1,
                            Key = key,
                            Type = typeFullName,
                            Value = keyAttribute.DefaultValue
                        };
                        await fairPlayCombinedDbContext.Resource.AddAsync(entity, stoppingToken);
                    }
                }
            }
        }
        if (fairPlayCombinedDbContext.ChangeTracker.HasChanges())
            await fairPlayCombinedDbContext.SaveChangesAsync(stoppingToken);
    }

    private static List<Type> GetTypesToCheck()
    {
        var fairplayTubeAssembly = typeof(FairPlayTube.Components.App).Assembly;
        var fairplayTubeTypes = fairplayTubeAssembly.GetTypes();

        var fairplayTubeSharedUIAssembly = typeof(FairPlayTube.SharedUI._Imports).Assembly;
        var fairplayTubeSharedUITypes = fairplayTubeSharedUIAssembly.GetTypes();

        var adminPortalAssembly = typeof(ApplicationUser).Assembly;
        var adminPortalTypes = adminPortalAssembly.GetTypes();

        var fairplaySocialAssembly = typeof(FairPlaySocial.Components.App).Assembly;
        var fairplaySocialTypes = fairplaySocialAssembly.GetTypes();

        var fairPlayDatingAssembly = typeof(FairPlayDating.Components.App).Assembly;
        var fairPlayDatingTypes = fairPlayDatingAssembly.GetTypes();

        var modelsAssembly = typeof(Models.UserModel).Assembly;
        var modelsTypes = modelsAssembly.GetTypes();

        var serverSideServicesAssembly = typeof(BaseService).Assembly;
        var serverSideServicesTypes = serverSideServicesAssembly.GetTypes();

        var commonAssembly = typeof(Common.Constants).Assembly;
        var commonTypes = commonAssembly.GetTypes();

        var fairplayCrmAssembly = typeof(FairPlayCRM.Components.App).Assembly;
        var fairplayCrmTypes = fairplayCrmAssembly.GetTypes();

        var fairplayCrmSharedUIAssembly = typeof(FairPlayCRM.SharedUI._Imports).Assembly;
        var fairplayCrmSharedUITypes = fairplayCrmSharedUIAssembly.GetTypes();

        var fairplayCombinedSharedAuthAssembly = typeof(SharedAuth._Imports).Assembly;
        var fairplayCombinedSharedAuthTypes = fairplayCombinedSharedAuthAssembly.GetTypes();

        var fairplayCombinedWebComponentsAssembly = typeof(WebComponents._Imports).Assembly;
        var fairplayCombinedWebComponentsTypes = fairplayCombinedWebComponentsAssembly.GetTypes();

        var fairplayBlogsAssembly = typeof(FairPlayBlogs.Components.App).Assembly;
        var fairplayBlogsTypes = fairplayBlogsAssembly.GetTypes();

        var fairplayBlogsSharedUIAssembly = typeof(FairPlayBlogs.SharedUI._Imports).Assembly;
        var fairplayBlogsSharedUITypes = fairplayBlogsSharedUIAssembly.GetTypes();

        List<Type> typesToCheck = [
            .. fairplayTubeTypes,
            .. fairplayTubeSharedUITypes,
            .. adminPortalTypes,
            .. modelsTypes,
            .. serverSideServicesTypes,
            .. commonTypes,
            .. fairplaySocialTypes,
            .. fairPlayDatingTypes,
            .. fairplayCrmTypes,
            .. fairplayCrmSharedUITypes,
            .. fairplayCombinedSharedAuthTypes,
            .. fairplayCombinedWebComponentsTypes,
            .. fairplayBlogsTypes,
            .. fairplayBlogsSharedUITypes];
        return typesToCheck.DistinctBy(p => p.FullName).ToList();
    }
}
