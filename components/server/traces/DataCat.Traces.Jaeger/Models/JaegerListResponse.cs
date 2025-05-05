namespace DataCat.Traces.Jaeger.Models;

internal record JaegerListResponse<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; init; } = [];

    [JsonPropertyName("total")]
    public int Total { get; init; }

    [JsonPropertyName("limit")]
    public int Limit { get; init; }

    [JsonPropertyName("offset")]
    public int Offset { get; init; }
}