namespace DataCat.Server.Application.Telemetry.Traces.Abstractions;

public interface ITracesClient : IDataSourceClient
{
    Task<IEnumerable<string>> GetServicesAsync(CancellationToken token = default);

    Task<IEnumerable<string>> GetOperationsAsync(
        string serviceName,
        CancellationToken token = default);

    Task<IEnumerable<TraceEntry>> FindTracesAsync(
        string service,
        DateTime start,
        DateTime end,
        string? operation = null,
        int? limit = null,
        TimeSpan? minDuration = null,
        TimeSpan? maxDuration = null,
        Dictionary<string, object>? tags = null,
        CancellationToken token = default);

    Task<TraceEntry?> GetTraceAsync(
        string traceId,
        CancellationToken token = default);
}
