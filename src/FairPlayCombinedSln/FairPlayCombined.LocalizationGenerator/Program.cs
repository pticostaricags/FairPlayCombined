using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.LocalizationGenerator;
using FairPlayCombined.Services.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>(
    optionsAction =>
    {
        optionsAction.UseSqlServer(connectionString,
            sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.UseNetTopologySuite();
                sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
            });
    });
builder.Services.AddAzureOpenAIService();
builder.Services.AddHostedService<LocalizationGenerator>();

var host = builder.Build();
host.Run();
