namespace DataCat.Server.Postgres.Snapshots;

public class PanelSnapshot
{
    public const string PanelTable = "panels";
    public const string DataSourceId = "data_source_id";
    
    public required string PanelId { get; init; }
    public required string Title { get; init; }
    public required int PanelType { get; init; }
    public required QuerySnapshot Query { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required string ParentDashboardId { get; init; }
}

public static class PanelEntitySnapshotMapper
{
    public static PanelSnapshot Save(this PanelEntity panelEntity)
    {
        return new PanelSnapshot
        {
            PanelId = panelEntity.Id.ToString(),
            Title = panelEntity.Title,
            PanelType = panelEntity.PanelType.Value,
            Query = panelEntity.QueryEntity.Save(),
            X = panelEntity.DataCatLayout.X,
            Y = panelEntity.DataCatLayout.Y,
            Width = panelEntity.DataCatLayout.Width,
            Height = panelEntity.DataCatLayout.Height,
            ParentDashboardId = panelEntity.ParentDashboardId.ToString()
        };
    }

    public static PanelEntity RestoreFromSnapshot(this PanelSnapshot snapshot)
    {
        var layout = DataCatLayout.Create(
            snapshot.X, snapshot.Y, snapshot.Width, snapshot.Height);

        if (layout.IsFailure)
            throw new DatabaseMappingException(typeof(DataSource));
        
        var result = PanelEntity.Create(
            Guid.Parse(snapshot.PanelId),
            snapshot.Title,
            PanelType.FromValue(snapshot.PanelType),
            snapshot.Query.RestoreFromSnapshot(),
            layout.Value,
            Guid.Parse(snapshot.ParentDashboardId));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSource));
    }
}