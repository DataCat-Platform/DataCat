namespace DataCat.Server.Telemetry;

public class DataCatExporterOptionSetup(
    IOptions<DataCatExporterOption> exporterSettings)
    : IConfigureOptions<OtlpExporterOptions>
{
    public void Configure(OtlpExporterOptions options)
    {
        var settings = exporterSettings.Value;
        
        options.Endpoint = new Uri(settings.Endpoint);
        options.TimeoutMilliseconds = settings.TimeoutMilliseconds;
        options.Protocol = settings.Protocol;
    }
}