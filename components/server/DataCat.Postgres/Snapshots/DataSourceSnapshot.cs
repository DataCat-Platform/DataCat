namespace DataCat.Server.Postgres.Snapshots;

public class DataSourceSnapshot
{
    public const string DataSourceTable = "data_sources";

    public required string DataSourceId { get; init; }
    public required string Name { get; init; }
    public required int DataSourceType { get; init; }
    public required string ConnectionString { get; init; }
}

public static class DataSourceEntitySnapshotMapper 
{
    public static DataSourceSnapshot Save(this DataSource dataSource)
    {
        return new DataSourceSnapshot
        {
            DataSourceId = dataSource.Id.ToString(),
            Name = dataSource.Name,
            DataSourceType = dataSource.DataSourceType.Value,
            ConnectionString = dataSource.ConnectionString
        };
    }

    public static DataSource RestoreFromSnapshot(this DataSourceSnapshot snapshot)
    {
        var result = DataSource.Create(
            Guid.Parse(snapshot.DataSourceId),
            snapshot.Name,
            DataSourceType.FromValue(snapshot.DataSourceType),
            snapshot.ConnectionString);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSource));
    }
}