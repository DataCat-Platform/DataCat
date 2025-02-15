namespace DataCat.OpenTelemetry.Exporter.Implementation;

[EventSource(Name = "OpenTelemetry-Exporter-DataCat")]
public sealed class DataCatExporterEventSource : EventSource
{
    internal static readonly DataCatExporterEventSource Log = new();
    
    [NonEvent]
    public void FailedExport(Exception ex)
    {
        if (IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            FailedExport(ex.ToInvariantString());
        }
    }
    
    [Event(1, Message = "Failed to export metric: '{0}'", Level = EventLevel.Error)]
    private void FailedExport(string exception)
    {
        WriteEvent(1, exception);
    }
    
    [Event(10, Message = "{0} has been disposed.", Level = EventLevel.Informational)]
    public void DisposedObject(string name) => WriteEvent(10, name);
    
    [Event(11, Message = "Failed to export metric: '{0}'. Unsupported metric type: '{1}'", Level = EventLevel.Informational)]
    public void UnsupportedMetricType(string name, string metric) => WriteEvent(11, name, metric);
}