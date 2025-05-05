namespace DataCat.Server.Domain.Core.Enums;

public abstract class DataSourcePurpose(string name, int value)
    : SmartEnum<DataSourcePurpose, int>(name, value)
{
    public static readonly DataSourcePurpose Metrics = new ForMetrics();
    public static readonly DataSourcePurpose Logs = new ForLogs();
    public static readonly DataSourcePurpose Traces = new ForTraces();

    private sealed class ForMetrics() : DataSourcePurpose("Metrics", 1);
    private sealed class ForLogs() : DataSourcePurpose("Logs", 2);
    private sealed class ForTraces() : DataSourcePurpose("Traces", 3);
}