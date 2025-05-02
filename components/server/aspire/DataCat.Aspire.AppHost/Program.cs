var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DataCat_Server_Api>("DataCat");

builder.Build().Run();