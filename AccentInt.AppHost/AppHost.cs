var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AccentInt_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
