namespace DataCat.Server.Telemetry;

public static class DependencyInjection
{
    public static IServiceCollection AddTelemetry(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder loggingBuilder)
    {
        if (Environment.GetEnvironmentVariable("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL") is not null)
        {
            return services;
        }
        
        services.AddOptions<DataCatExporterOption>()
            .Bind(configuration.GetSection(DataCatExporterOption.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.ConfigureOptions<DataCatExporterOptionSetup>();
        
        loggingBuilder.AddOpenTelemetry(options => 
        {
            options.IncludeScopes = true;
            options.ParseStateValues = true;
        });
        
        services.AddOpenTelemetry()
            .ConfigureResource(configure => configure
                .AddService(MetricsConstants.ServiceName))
            .WithMetrics(configure => configure
                .AddMeter(MetricsConstants.MeterName)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter())
            .WithLogging(configure => configure
                .AddOtlpExporter());
        
        return services;
    }
}