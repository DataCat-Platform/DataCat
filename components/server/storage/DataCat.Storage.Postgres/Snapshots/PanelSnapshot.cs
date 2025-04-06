namespace DataCat.Storage.Postgres.Snapshots;

public sealed record PanelSnapshot
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required int TypeId { get; init; }
    public required string RawQuery { get; init; }
    public required DataSourceSnapshot DataSource { get; set; }
    public string DataSourceId => DataSource.Id;
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required string DashboardId { get; init; }
}

public static class PanelEntitySnapshotMapper
{
    public static PanelSnapshot Save(this PanelEntity panelEntity)
    {
        var query = panelEntity.QueryEntity.Save();
        
        return new PanelSnapshot
        {
            Id = panelEntity.Id.ToString(),
            Title = panelEntity.Title,
            TypeId = panelEntity.Type.Value,
            RawQuery = query.PanelRawQuery,
            DataSource = query.DataSource,
            X = panelEntity.DataCatLayout.X,
            Y = panelEntity.DataCatLayout.Y,
            Width = panelEntity.DataCatLayout.Width,
            Height = panelEntity.DataCatLayout.Height,
            DashboardId = panelEntity.DashboardId.ToString()
        };
    }

    public static PanelEntity RestoreFromSnapshot(this PanelSnapshot snapshot)
    {
        var layout = DataCatLayout.Create(
            snapshot.X,
            snapshot.Y,
            snapshot.Width,
            snapshot.Height).Value;

        var dataSource = snapshot.DataSource.RestoreFromSnapshot();

        var query = QueryEntity.Create(dataSource, snapshot.RawQuery).Value;
        
        var result = PanelEntity.Create(
            Guid.Parse(snapshot.Id),
            snapshot.Title,
            PanelType.FromValue(snapshot.TypeId),
            query,
            layout,
            Guid.Parse(snapshot.DashboardId));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSourceEntity));
    }
}