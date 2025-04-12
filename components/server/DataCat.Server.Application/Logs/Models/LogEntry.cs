namespace DataCat.Server.Application.Logs.Models;

/// <summary>
/// Represents a single log entry in the system.
/// </summary>
/// <remarks>
/// This record encapsulates all relevant information for a log entry including
/// timestamp, message, severity, and contextual information like trace ID.
/// Additional fields can be stored in the <see cref="AdditionalFields"/> dictionary.
/// </remarks>
/// <param name="Timestamp">The exact date and time when the log entry was created.</param>
/// <param name="Message">The main log message content.</param>
/// <param name="Severity">The severity level of the log entry (e.g., Error, Warning, Information, see <see cref="LogSeverity"/>).</param>
/// <param name="ServiceName">The name of the service or application that generated the log.</param>
/// <param name="TraceId">The correlation ID for distributed tracing (usually a GUID).</param>
/// <param name="AdditionalFields">Additional structured data associated with the log entry.
/// <para>
/// Typical keys might include:
/// - "Environment" (prod/stage/dev)
/// - "Host" (machine name)
/// - "ExceptionType" (for error logs)
/// - Custom business context fields
/// </para>
/// </param>
public sealed record LogEntry(
    DateTime Timestamp,
    string Message,
    string Severity,
    string ServiceName,
    string TraceId,
    Dictionary<string, object?> AdditionalFields);
