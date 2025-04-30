namespace DataCat.Server.Application.Enums;

public enum DataSourceKind
{
    Metrics,
    Logs,
    Traces
}

public static class DataSourceKindExtensions
{
    public static DataSourceKind DetermineKind(this DataSourcePurpose purpose)
    {
        if (purpose == DataSourcePurpose.Metrics)
            return DataSourceKind.Metrics;
        
        return purpose == DataSourcePurpose.Logs 
            ? DataSourceKind.Logs 
            : DataSourceKind.Traces;
    }
}
