using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DatabaseManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<FairPlayCombinedDbContext>("FairPlayCombinedDb",
    configureDbContextOptions: (options) =>
    {
        options.UseSqlServer(builder =>
        {
            builder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
            builder.UseNetTopologySuite();
        });
    });
builder.Services.AddSingleton<FairPlayCombinedDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<FairPlayCombinedDbInitializer>());

builder.Services.AddHealthChecks()
    .AddCheck<FairPlayCombinedDbInitializerHealthCheck>("DbInitializer", null);

var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();
