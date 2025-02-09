namespace DataCat.Server.Postgres.Snapshots;

public class QuerySnapshot
{
    public required string PanelRawQuery { get; init; }
    public required DataSourceSnapshot DataSource { get; init; }        
}

public static class QueryEntitySnapshotMapper
{
    public static QuerySnapshot Save(this QueryEntity query)
    {
        return new QuerySnapshot
        {
            PanelRawQuery = query.RawQuery,
            DataSource = query.DataSourceEntity.Save(),
        };
    }

    public static QueryEntity RestoreFromSnapshot(this QuerySnapshot snapshot)
    {
        var result = QueryEntity.Create(
            snapshot.DataSource.RestoreFromSnapshot(),
            snapshot.PanelRawQuery);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSourceEntity));
    }
}