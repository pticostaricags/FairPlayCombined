using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlayTube;
using FairPlayTube.VideoIndexing;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var azureVideoIndexerAccountId = Environment.GetEnvironmentVariable("AzureVideoIndexerAccountId") ??
    throw new InvalidOperationException("'AzureVideoIndexerAccountId' not found");
var azureVideoIndexerLocation = Environment.GetEnvironmentVariable("AzureVideoIndexerLocation") ??
    throw new InvalidOperationException("'AzureVideoIndexerLocation' not found");
var azureVideoIndexerResourceGroup = Environment.GetEnvironmentVariable("AzureVideoIndexerResourceGroup") ??
    throw new InvalidOperationException("'AzureVideoIndexerResourceGroup' not found");
var azureVideoIndexerResourceName = Environment.GetEnvironmentVariable("AzureVideoIndexerResourceName") ??
    throw new InvalidOperationException("'AzureVideoIndexerResourceName' not found");
var azureVideoIndexerSubscriptionId = Environment.GetEnvironmentVariable("AzureVideoIndexerSubscriptionId") ??
    throw new InvalidOperationException("'AzureVideoIndexerSubscriptionId' not found");
AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration = new()
{
    AccountId = azureVideoIndexerAccountId,
    IsArmAccount = true,
    Location = azureVideoIndexerLocation,
    ResourceGroup = azureVideoIndexerResourceGroup,
    ResourceName = azureVideoIndexerResourceName,
    SubscriptionId = azureVideoIndexerSubscriptionId,
};
var connectionString = Environment.GetEnvironmentVariable("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayTube), ref connectionString);

builder.Services.AddTransient<IUserProviderService, VideoIndexingUserProviderService>();
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>(
    (sp, optionsAction) =>
    {
        IUserProviderService userProviderService = sp.GetRequiredService<IUserProviderService>();
        optionsAction.AddInterceptors(new SaveChangesInterceptor(userProviderService));
        optionsAction.UseSqlServer(connectionString,
            sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.UseNetTopologySuite();
                sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
            });
    });
builder.Services.AddSingleton(azureVideoIndexerServiceConfiguration);
builder.Services.AddTransient(sp =>
{
    return new AzureVideoIndexerService(azureVideoIndexerServiceConfiguration,
        new HttpClient());
});
builder.Services.AddTransient<VideoInfoService>();


builder.Services.AddHostedService<VideoIndexStatusService>();

var host = builder.Build();
host.Run();
