var builder = DistributedApplication.CreateBuilder(args);

//For issue about same target path, check: https://github.com/dotnet/aspire/issues/851
builder.AddProject<Projects.FairPlayDating>("fairplaydating");

builder.AddProject<Projects.FairPlayShop>("fairplayshop");

builder.AddProject<Projects.FairPlayTube>("fairplaytube");

builder.Build().Run();
