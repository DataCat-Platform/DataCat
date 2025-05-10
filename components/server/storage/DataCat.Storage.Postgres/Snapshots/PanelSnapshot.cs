namespace DataCat.Storage.Postgres.Snapshots;

public sealed record PanelSnapshot
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required int TypeId { get; init; }
    public required string RawQuery { get; init; }
    public required DataSourceSnapshot DataSource { get; set; }
    public string DataSourceId => DataSource.Id;
    public required string LayoutConfiguration { get; init; }
    public required string DashboardId { get; init; }
    public required string? StyleConfiguration { get; init; }
    public required string NamespaceId { get; init; }
}

public static class PanelEntitySnapshotExtensions
{
    public static PanelSnapshot Save(this Panel panel)
    {
        var query = panel.Query.Save();
        
        return new PanelSnapshot
        {
            Id = panel.Id.ToString(),
            Title = panel.Title,
            TypeId = panel.Type.Value,
            RawQuery = query.PanelRawQuery,
            DataSource = query.DataSource,
            DashboardId = panel.DashboardId.ToString(),
            StyleConfiguration = panel.StyleConfiguration,
            LayoutConfiguration = panel.DataCatLayout.Settings,
            NamespaceId = panel.NamespaceId.ToString(),
        };
    }

    public static Panel RestoreFromSnapshot(this PanelSnapshot snapshot)
    {
        var dataSource = snapshot.DataSource.RestoreFromSnapshot();

        var query = Query.Create(dataSource, snapshot.RawQuery).Value;
        
        var result = Panel.Create(
            Guid.Parse(snapshot.Id),
            snapshot.Title,
            PanelType.FromValue(snapshot.TypeId),
            query,
            new DataCatLayout(snapshot.LayoutConfiguration),
            Guid.Parse(snapshot.DashboardId),
            snapshot.StyleConfiguration,
            Guid.Parse(snapshot.NamespaceId));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSource));
    }
}