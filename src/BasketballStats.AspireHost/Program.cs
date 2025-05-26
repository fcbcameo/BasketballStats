var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BasketballStats_Web>("web");

builder.Build().Run();
