using FairPlayCombined.CitiesImporter;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<CitiesImporter>();

var host = builder.Build();
host.Run();
