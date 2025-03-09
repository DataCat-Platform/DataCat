namespace DataCat.Storage.Postgres.Snapshots;

public sealed class PanelSnapshot
{
    public const string PanelTable = "panels";
    public const string Panel_DataSourceId = "panel_data_source_id";
    
    public required string PanelId { get; init; }
    public required string PanelTitle { get; init; }
    public required int PanelType { get; init; }
    public required string PanelRawQuery { get; init; }
    public required DataSourceSnapshot PanelDataSource { get; set; }
    public string PanelDataSourceId => PanelDataSource.DataSourceId;
    public required int PanelX { get; init; }
    public required int PanelY { get; init; }
    public required int PanelWidth { get; init; }
    public required int PanelHeight { get; init; }
    public required string PanelParentDashboardId { get; init; }
}

public static class PanelEntitySnapshotMapper
{
    public static PanelSnapshot ReadPanel(this DbDataReader reader)
    {
        return new PanelSnapshot
        {
            PanelId = reader.GetString(reader.GetOrdinal(Public.Panels.PanelId)),
            PanelTitle = reader.GetString(reader.GetOrdinal(Public.Panels.PanelTitle)),
            PanelType = reader.GetInt32(reader.GetOrdinal(Public.Panels.PanelType)),
            PanelRawQuery = reader.GetString(reader.GetOrdinal(Public.Panels.PanelRawQuery)),
            PanelDataSource = new DataSourceSnapshot
            {
                DataSourceId = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceId)),
                DataSourceName = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceName)),
                DataSourceType = reader.GetInt32(reader.GetOrdinal(Public.DataSources.DataSourceType)),
                DataSourceConnectionString = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceConnectionString)),
            },
            PanelX = reader.GetInt32(reader.GetOrdinal(Public.Panels.PanelX)),
            PanelY = reader.GetInt32(reader.GetOrdinal(Public.Panels.PanelY)),
            PanelWidth = reader.GetInt32(reader.GetOrdinal(Public.Panels.PanelWidth)),
            PanelHeight = reader.GetInt32(reader.GetOrdinal(Public.Panels.PanelHeight)),
            PanelParentDashboardId = reader.GetString(reader.GetOrdinal(Public.Panels.PanelParentDashboardId))
        };
    }
    
    public static PanelSnapshot Save(this PanelEntity panelEntity)
    {
        var query = panelEntity.QueryEntity.Save();
        
        return new PanelSnapshot
        {
            PanelId = panelEntity.Id.ToString(),
            PanelTitle = panelEntity.Title,
            PanelType = panelEntity.PanelType.Value,
            PanelRawQuery = query.PanelRawQuery,
            PanelDataSource = query.DataSource,
            PanelX = panelEntity.DataCatLayout.X,
            PanelY = panelEntity.DataCatLayout.Y,
            PanelWidth = panelEntity.DataCatLayout.Width,
            PanelHeight = panelEntity.DataCatLayout.Height,
            PanelParentDashboardId = panelEntity.ParentDashboardId.ToString()
        };
    }

    public static PanelEntity RestoreFromSnapshot(this PanelSnapshot snapshot)
    {
        var layout = DataCatLayout.Create(
            snapshot.PanelX,
            snapshot.PanelY,
            snapshot.PanelWidth,
            snapshot.PanelHeight).Value;

        var dataSource = snapshot.PanelDataSource.RestoreFromSnapshot();

        var query = QueryEntity.Create(dataSource, snapshot.PanelRawQuery).Value;
        
        var result = PanelEntity.Create(
            Guid.Parse(snapshot.PanelId),
            snapshot.PanelTitle,
            PanelType.FromValue(snapshot.PanelType),
            query,
            layout,
            Guid.Parse(snapshot.PanelParentDashboardId));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSourceEntity));
    }
}