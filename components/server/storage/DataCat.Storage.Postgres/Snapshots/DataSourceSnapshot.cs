namespace DataCat.Storage.Postgres.Snapshots;

public sealed record DataSourceSnapshot
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required int TypeId { get; init; }
    public required string ConnectionString { get; init; }
}

public static class DataSourceEntitySnapshotMapper 
{
    public static DataSourceSnapshot Save(this DataSourceEntity dataSourceEntity)
    {
        return new DataSourceSnapshot
        {
            Id = dataSourceEntity.Id.ToString(),
            Name = dataSourceEntity.Name,
            TypeId = dataSourceEntity.DataSourceType.Value,
            ConnectionString = dataSourceEntity.ConnectionString
        };
    }

    public static DataSourceEntity RestoreFromSnapshot(this DataSourceSnapshot snapshot)
    {
        var result = DataSourceEntity.Create(
            Guid.Parse(snapshot.Id),
            snapshot.Name,
            DataSourceType.FromValue(snapshot.TypeId),
            snapshot.ConnectionString);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSourceEntity));
    }
}