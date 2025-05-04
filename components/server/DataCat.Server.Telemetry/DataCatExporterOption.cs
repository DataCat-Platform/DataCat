namespace DataCat.Server.Telemetry;

public sealed record DataCatExporterOption
{
    public const string SectionName = "DataCatTelemetryOptions";
    
    public string Endpoint { get; init; } = "http://localhost:4317";
    
    public int TimeoutMilliseconds { get; init; } = 10000;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OtlpExportProtocol Protocol { get; init; } = OtlpExportProtocol.Grpc;
    
    public Dictionary<string, string> Headers { get; init; } = new();
}