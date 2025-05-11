namespace DataCat.Storage.Postgres.Snapshots;

public sealed record PanelTypeSnapshot
{
    public const string PanelTypeTable = "panel_types";

    public required int Id { get; init; }
    public required string Type { get; init; }
}

public static class PanelTypeEntitySnapshotMapper 
{
    public static PanelTypeSnapshot Save(this PanelType panelType)
    {
        return new PanelTypeSnapshot
        {
            Id = panelType.Value,
            Type = panelType.Name
        };
    }

    public static PanelType RestoreFromSnapshot(this PanelTypeSnapshot snapshot)
    {
        var result = PanelType.FromValue(snapshot.Id);
        return result ?? throw new DatabaseMappingException(typeof(PanelType));
    }
}