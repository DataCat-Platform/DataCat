namespace DataCat.OpenTelemetry.Exporter;

/// <summary>
/// Extension methods to simplify registering of DataCat exporter.
/// </summary>
public static class DataCatExporterHelperExtensions
{
    private const string DataCatDefaultName = "DataCat";
    
    /// <summary>
    /// Adds DataCat exporter to the MeterProvider.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> builder to use.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddDataCatExporter(this MeterProviderBuilder builder)
        => AddDataCatExporter(builder, name: null, configure: null);

    /// <summary>
    /// Adds DataCat exporter to the MeterProvider.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> builder to use.</param>
    /// <param name="configure">Callback action for configuring <see cref="DataCatExporterOptions"/>.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddDataCatExporter(this MeterProviderBuilder builder, Action<DataCatExporterOptions> configure)
        => AddDataCatExporter(builder, name: null, configure);

    /// <summary>
    /// Adds DataCat exporter to the MeterProvider.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> builder to use.</param>
    /// <param name="name">Optional name which is used when retrieving options.</param>
    /// <param name="configure">Optional callback action for configuring <see cref="DataCatExporterOptions"/>.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddDataCatExporter(
        this MeterProviderBuilder builder,
        string? name,
        Action<DataCatExporterOptions>? configure)
    {
        NullGuard.ThrowIfNull(builder);

        name ??= DataCatDefaultName;

        builder.ConfigureServices(services =>
        {
            if (configure != null)
            {
                services.Configure(name, configure);
            }
        });

        return builder.AddReader(sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<DataCatExporterOptions>>().Get(name);

            if (name is null && configure is not null)
            {
                configure?.Invoke(options);
            }

            return new PeriodicExportingMetricReader(
                new DataCatExporter(options),
                exportIntervalMilliseconds: 10_000);
        });
    }
}