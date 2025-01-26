var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
var configuration = builder.Configuration;

builder.Services.AddGrpc();
builder.Services
    .AddCustomMiddlewares()
    .AddApiSetup()
    .AddApplicationServices(configuration)
    .AddServerLogging(configuration)
    .AddMigrationSetup(configuration)
    .AddRealTimeCommunication(configuration);

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

var app = builder.Build();

if (bool.TryParse(app.Configuration["ApplyMigrations"], out var applyMigrations) && applyMigrations)
{
    await app.ApplyMigrations();
}

app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});

app.UseRouting();
app.UseLoggingRequests();
app.UseExceptionHandling();

app.MapHub<MetricsHub>("/datacat-metrics");
app.MapGrpcService<ReceiveMetricService>();
app.MapControllers();

app.MapScalarApiReference("/scalar");
app.UseEndpoints(_ => { });

#if DEBUG
    app.UseCors("frontend");
    app.UseSpa(cfg =>
    {
        cfg.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    });
#else
    app.UseStaticFiles();
    app.UseSpa(cfg => cfg.Options.SourcePath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp"));
#endif

app.Run();
