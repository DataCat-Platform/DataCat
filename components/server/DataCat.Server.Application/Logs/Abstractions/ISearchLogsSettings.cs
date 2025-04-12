namespace DataCat.Server.Application.Logs.Abstractions;

public interface ISearchLogsSettings
{
    string ClusterUrl { get; }
    string IndexPattern { get; }
    TimeSpan RequestTimeout { get; }
    string UserName { get; }
    string Password { get; }
    bool EnableDebugLogging { get; }
}