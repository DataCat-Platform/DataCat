namespace DataCat.Server.Application.Telemetry.Logs.Models;

/// <summary>
/// Standard log severity levels
/// </summary>
public abstract class LogSeverity(string name, int value)
    : SmartEnum<LogSeverity, int>(name, value)
{
    public static readonly LogSeverity Verbose = new VerboseLogSeverity();
    public static readonly LogSeverity Debug = new DebugLogSeverity();
    public static readonly LogSeverity Information = new InformationLogSeverity();
    public static readonly LogSeverity Warning = new WarningLogSeverity();
    public static readonly LogSeverity Error = new ErrorLogSeverity();
    public static readonly LogSeverity Critical = new CriticalLogSeverity();

    private sealed class VerboseLogSeverity() : LogSeverity("Verbose", 1);
    private sealed class DebugLogSeverity() : LogSeverity("Debug", 2);
    private sealed class InformationLogSeverity() : LogSeverity("Information", 3);
    private sealed class WarningLogSeverity() : LogSeverity("Warning", 4);
    private sealed class ErrorLogSeverity() : LogSeverity("Error", 5);
    private sealed class CriticalLogSeverity() : LogSeverity("Critical", 6);
}