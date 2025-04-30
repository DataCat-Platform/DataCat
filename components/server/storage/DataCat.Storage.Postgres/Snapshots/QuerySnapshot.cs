namespace DataCat.Storage.Postgres.Snapshots;

public sealed record QuerySnapshot
{
    public required string PanelRawQuery { get; init; }
    public required DataSourceSnapshot DataSource { get; init; }        
}

public static class QueryEntitySnapshotMapper
{
    public static QuerySnapshot Save(this Query query)
    {
        return new QuerySnapshot
        {
            PanelRawQuery = query.RawQuery,
            DataSource = query.DataSource.Save(),
        };
    }

    public static Query RestoreFromSnapshot(this QuerySnapshot snapshot)
    {
        var result = Query.Create(
            snapshot.DataSource.RestoreFromSnapshot(),
            snapshot.PanelRawQuery);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSource));
    }
}