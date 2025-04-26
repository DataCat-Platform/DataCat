namespace DataCat.Server.Application.Telemetry;

public sealed class DataSourceContainer
{
    private readonly ConcurrentDictionary<string, DataSource> _metrics = new();
    private readonly ConcurrentDictionary<string, DataSource> _logs = new();
    private readonly ConcurrentDictionary<string, DataSource> _traces = new();
    
    public void Load(DataSourceKind kind, IEnumerable<DataSource> dataSources)
    {
        var dictionary = GetDictionary(kind);
        dictionary.Clear();
        
        foreach (var dataSource in dataSources)
        {
            dictionary.TryAdd(dataSource.Name, dataSource);
        }
    }
    
    public bool Add(DataSourceKind kind, DataSource dataSource)
    {
        var dictionary = GetDictionary(kind);
        return dictionary.TryAdd(dataSource.Name, dataSource);
    }
    
    public DataSource? Find(DataSourceKind kind, string name)
    {
        var dictionary = GetDictionary(kind);
        dictionary.TryGetValue(name, out var dataSource);
        return dataSource;
    }

    public bool Remove(DataSourceKind kind, string name)
    {
        var dictionary = GetDictionary(kind);
        return dictionary.TryRemove(name, out _);
    }

    public IReadOnlyCollection<DataSource> GetAll(DataSourceKind kind)
    {
        var dictionary = GetDictionary(kind);
        return dictionary.Values.ToList().AsReadOnly();
    }

    private ConcurrentDictionary<string, DataSource> GetDictionary(DataSourceKind kind) => kind switch
    {
        DataSourceKind.Metrics => _metrics,
        DataSourceKind.Logs => _logs,
        DataSourceKind.Traces => _traces,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported data source kind")
    };
}