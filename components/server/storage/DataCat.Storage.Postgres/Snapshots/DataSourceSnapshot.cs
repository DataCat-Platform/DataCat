namespace DataCat.Storage.Postgres.Snapshots;

public sealed record DataSourceSnapshot
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required DataSourceTypeSnapshot DataSourceType { get; set; }
    public int? TypeId => DataSourceType.Id;
    public required string ConnectionSettings { get; init; }
    public required string Purpose { get; init; } 
}

public static class DataSourceEntitySnapshotMapper 
{
    public static DataSourceSnapshot Save(this DataSource dataSource)
    {
        return new DataSourceSnapshot
        {
            Id = dataSource.Id.ToString(),
            Name = dataSource.Name,
            DataSourceType = dataSource.DataSourceType.Save(),
            ConnectionSettings = dataSource.ConnectionSettings,
            Purpose = dataSource.Purpose.Name
        };
    }

    public static DataSource RestoreFromSnapshot(this DataSourceSnapshot snapshot)
    {
        var result = DataSource.Create(
            Guid.Parse(snapshot.Id),
            snapshot.Name,
            snapshot.DataSourceType.RestoreFromSnapshot(),
            snapshot.ConnectionSettings,
            DataSourcePurpose.FromName(snapshot.Purpose));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSource));
    }
}