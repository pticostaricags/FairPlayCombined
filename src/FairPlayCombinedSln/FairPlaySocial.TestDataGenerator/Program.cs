using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlaySocial.TestDataGenerator;
using FairPlaySocial.TestDataGenerator.Providers;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
var connectionString = Environment.GetEnvironmentVariable("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>(optionsAction =>
{
    optionsAction.AddInterceptors(new SaveChangesInterceptor(new UserProviderService()));
    optionsAction.UseSqlServer(connectionString,
        sqlServerOptionsAction =>
        {
            sqlServerOptionsAction.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
            sqlServerOptionsAction.UseNetTopologySuite();
            sqlServerOptionsAction.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(3),
                errorNumbersToAdd: null);
        });
});
builder.Services.AddHostedService<TestDataGenerator>();

var host = builder.Build();
host.Run();
