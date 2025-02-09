namespace DataCat.Server.Postgres.Snapshots;

public class DataSourceSnapshot
{
    public const string DataSourceTable = "data_sources";

    public required string DataSourceId { get; init; }
    public required string DataSourceName { get; init; }
    public required int DataSourceType { get; init; }
    public required string DataSourceConnectionString { get; init; }
}

public static class DataSourceEntitySnapshotMapper 
{
    public static DataSourceSnapshot ReadDataSource(this DbDataReader reader)
    {
        return new DataSourceSnapshot
        {
            DataSourceId = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceId)),
            DataSourceName = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceName)),
            DataSourceType = reader.GetInt32(reader.GetOrdinal(Public.DataSources.DataSourceType)),
            DataSourceConnectionString = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceConnectionString)),
        };
    }
    
    public static DataSourceSnapshot Save(this DataSourceEntity dataSourceEntity)
    {
        return new DataSourceSnapshot
        {
            DataSourceId = dataSourceEntity.Id.ToString(),
            DataSourceName = dataSourceEntity.Name,
            DataSourceType = dataSourceEntity.DataSourceType.Value,
            DataSourceConnectionString = dataSourceEntity.ConnectionString
        };
    }

    public static DataSourceEntity RestoreFromSnapshot(this DataSourceSnapshot snapshot)
    {
        var result = DataSourceEntity.Create(
            Guid.Parse(snapshot.DataSourceId),
            snapshot.DataSourceName,
            DataSourceType.FromValue(snapshot.DataSourceType),
            snapshot.DataSourceConnectionString);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSourceEntity));
    }
}