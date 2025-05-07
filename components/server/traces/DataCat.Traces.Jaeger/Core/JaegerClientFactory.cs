namespace DataCat.Traces.Jaeger.Core;

public sealed class JaegerClientFactory : ITracesClientFactory
{
    private static readonly JsonSerializerOptions serializationOptions = new() { PropertyNameCaseInsensitive = true };
    
    public bool CanCreate(DataSource dataSource)
    {
        return dataSource.Purpose == DataSourcePurpose.Traces
               && string.Equals(dataSource.DataSourceType.Name, JaegerConstants.Jaeger, StringComparison.OrdinalIgnoreCase);
    }

    public ITracesClient CreateClient(DataSource dataSource)
    {
        if (string.IsNullOrWhiteSpace(dataSource.ConnectionSettings))
        {
            throw new ArgumentException("ConnectionSettings must be provided for Jaeger DataSource", nameof(dataSource));
        }
        
        JaegerSettings settings;
        try
        {
            settings = JsonSerializer.Deserialize<JaegerSettings>(dataSource.ConnectionSettings, serializationOptions)
                       ?? throw new InvalidOperationException("JaegerSettings deserialized as null");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse JaegerSettings from ConnectionSettings", ex);
        }
        
        var client = new HttpClient();
        
        return new JaegerClient(client, settings);
    }
}
