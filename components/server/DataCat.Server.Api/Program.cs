var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
var configuration = builder.Configuration;

builder.Services.AddGrpc();
builder.Services
    .AddCustomMiddlewares()
    .AddApiSetup()
    .AddApplicationServices()
    .AddServerLogging(configuration)
    .AddMigrationSetup(configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });

    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

if (bool.TryParse(app.Configuration["ApplyMigrations"], out var applyMigrations) && applyMigrations)
{
    await app.ApplyMigrations();
}

app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});
app.MapScalarApiReference();
app.UseSwaggerUI();

app
    .UseLoggingRequests()
    .UseExceptionHandling();

app.MapGrpcService<ReceiveMetricService>();
app.MapControllers();

app.Run();
