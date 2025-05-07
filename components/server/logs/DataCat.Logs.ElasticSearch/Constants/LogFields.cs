namespace DataCat.Logs.ElasticSearch.Constants;

public static class LogFields
{
    public const string TraceId = "TraceId";
    public const string Timestamp = "@timestamp";
    public const string Level = "level.keyword";
    public const string Host = "fields.Host.keyword";
}