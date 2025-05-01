namespace DataCat.Traces.Jaeger.Models;

internal sealed record JaegerProcess
{
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; init; } = string.Empty;

    [JsonPropertyName("tags")]
    public List<JaegerTag> Tags { get; init; } = [];
}