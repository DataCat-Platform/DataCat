namespace DataCat.OpenTelemetry.Exporter.Example.Metrics;

public class MyMetric
{
    public static readonly string GlobalSystemName = Environment.MachineName;
    public static readonly string AppName = AppDomain.CurrentDomain.FriendlyName;
    public static readonly string InstrumentSourceName = nameof(MyMetric);
    
    public Counter<int> MyCounter { get; set; }
    
    public MyMetric(IMeterFactory meterFactory)
    {
        MyCounter = meterFactory.Create(InstrumentSourceName, version: "1.0.0")
            .CreateCounter<int>(name: "test_counter.requests",
                unit: "Requests",
                description: "The number of requests received");
    }
}