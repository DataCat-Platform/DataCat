namespace DataCat.Storage.Postgres.Snapshots;

public sealed record DataSourceTypeSnapshot
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public static class DataSourceTypeEntitySnapshotMapper 
{
    public static DataSourceTypeSnapshot Save(this DataSourceType dataSourceType)
    {
        return new DataSourceTypeSnapshot
        {
            Id = dataSourceType.Id,
            Name = dataSourceType.Name
        };
    }

    public static DataSourceType RestoreFromSnapshot(this DataSourceTypeSnapshot snapshot)
    {
        var result = DataSourceType.Create(snapshot.Name, snapshot.Id).Value;
        return result;
    }
}