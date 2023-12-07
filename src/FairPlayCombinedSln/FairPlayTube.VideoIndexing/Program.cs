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
var connectionString = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayTube.VideoIndexing), ref connectionString);

builder.Services.AddTransient<IUserProviderService, VideoIndexingUserProviderService>();
builder.Services.AddTransient<DbContextOptions<FairPlayCombinedDbContext>>(sp =>
{
    IUserProviderService userProviderService = sp.GetRequiredService<IUserProviderService>();
    DbContextOptionsBuilder<FairPlayCombinedDbContext> optionsBuilder = new();
    optionsBuilder.AddInterceptors(new SaveChangesInterceptor(userProviderService));
    optionsBuilder.UseSqlServer(connectionString,
        sqlServerOptionsAction =>
        {
            sqlServerOptionsAction.UseNetTopologySuite();
            sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    return optionsBuilder.Options;
});
builder.AddSqlServerDbContext<FairPlayCombinedDbContext>(connectionName: "FairPlayCombinedDb");
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>();
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
