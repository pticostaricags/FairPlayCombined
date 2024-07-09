using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataExportService;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Services.FairPlayTube;
using Microsoft.EntityFrameworkCore;
using SendGrid;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddAzureBlobClient("blobs");

if (Convert.ToBoolean(builder.Configuration["UseSendGrid"]))
{
    builder.Services.AddTransient<SendGridClient>(sp =>
    {
        var apiKey = builder.Configuration["SMTPPassword"];
        SendGridClient sendGridClient = new(apiKey: apiKey);
        return sendGridClient;
    });

    var connectionString = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
    Extensions.EnhanceConnectionString(nameof(FairPlayCombined.DataExportService), ref connectionString);

    builder.Services.AddTransient<IUserProviderService, DataExportUserProviderService>();
    builder.Services.AddDbContext<FairPlayCombinedDbContext>((sp, builder) =>
    {
        IUserProviderService userProviderService = sp.GetRequiredService<IUserProviderService>();
        builder.AddInterceptors(new SaveChangesInterceptor(userProviderService));
        builder.UseSqlServer(connectionString,
            sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.UseNetTopologySuite();
                sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
    }, contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);
    builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>();
    builder.EnrichSqlServerDbContext<FairPlayCombinedDbContext>();
    builder.Services.AddTransient<IFairPlayTubeUserDataService, FairPlayTubeUserDataService>();
    builder.Services.AddHostedService<DataExportBackgroundService>();

    var host = builder.Build();
    host.Run();
}