namespace DataCat.Traces.Jaeger.Models;

internal sealed record JaegerTag
{
    [JsonPropertyName("key")]
    public string Key { get; init; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("value")]
    public JsonElement Value { get; init; } = default!;
}