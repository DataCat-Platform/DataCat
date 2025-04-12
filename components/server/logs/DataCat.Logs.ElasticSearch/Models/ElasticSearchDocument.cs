namespace DataCat.Logs.ElasticSearch.Models;

public class ElasticSearchDocument
{
    [JsonPropertyName("@timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("level")]
    public string Level { get; set; } = null!;

    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [JsonPropertyName("fields")]
    public Dictionary<string, object?> Fields { get; set; } = null!;
}

public static class ElasticSearchDocumentExtensions
{
    public static LogEntry FromElasticDocument(this ElasticSearchDocument doc)
    {
        return new LogEntry(
            Timestamp: doc.Timestamp,
            Message: doc.Message,
            Severity: doc.Level,
            ServiceName: doc.Fields.TryGetValue("Host", out var host) && host is not null ? host.ToString()! : "unknown",
            AdditionalFields: doc.Fields.ToDictionary(x => x.Key, x => x.Value),
            TraceId: doc.Fields.TryGetValue("TraceId", out var traceId) && traceId is not null ? traceId.ToString()! : "unknown"
        );
    }
}