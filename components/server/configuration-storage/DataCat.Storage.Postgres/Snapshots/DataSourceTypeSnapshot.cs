namespace DataCat.Storage.Postgres.Snapshots;

public sealed class DataSourceTypeSnapshot
{
    public const string DataSourceTypeTable = "data_source_types";

    public required int Id { get; init; }
    public required string Source { get; init; }
}

public static class DataSourceTypeEntitySnapshotMapper 
{
    public static DataSourceTypeSnapshot Save(this DataSourceType dataSourceType)
    {
        return new DataSourceTypeSnapshot
        {
            Id = dataSourceType.Value,
            Source = dataSourceType.Name
        };
    }

    public static DataSourceType RestoreFromSnapshot(this DataSourceTypeSnapshot snapshot)
    {
        var result = DataSourceType.FromValue(snapshot.Id);
        return result ?? throw new DatabaseMappingException(typeof(DataSourceType));
    }
}