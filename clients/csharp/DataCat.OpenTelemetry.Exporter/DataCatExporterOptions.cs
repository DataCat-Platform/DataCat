namespace DataCat.OpenTelemetry.Exporter;

public sealed class DataCatExporterOptions 
{
    /// <summary>
    /// Server address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Grpc channel options
    /// </summary>
    public GrpcChannelOptions? ChannelOptions { get; set; }
}