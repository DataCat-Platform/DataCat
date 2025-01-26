namespace DataCat.Server.Postgres.Snapshots;

public class QuerySnapshot
{
    public required string RawQuery { get; init; }
    public required DataSourceSnapshot DataSource { get; init; }        
}

public static class QueryEntitySnapshotMapper
{
    public static QuerySnapshot Save(this QueryEntity query)
    {
        return new QuerySnapshot
        {
            RawQuery = query.RawQuery,
            DataSource = query.DataSource.Save(),
        };
    }

    public static QueryEntity RestoreFromSnapshot(this QuerySnapshot snapshot)
    {
        var result = QueryEntity.Create(
            snapshot.DataSource.RestoreFromSnapshot(),
            snapshot.RawQuery);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSource));
    }
}