namespace DataCat.Server.Postgres.Snapshots;

public class DashboardSnapshot
{
    public const string DashboardTable = "dashboards";
    public const string OwnerId = "owner_id";

    public required string DashboardId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required UserSnapshot Owner { get; init; }
    public required IEnumerable<PanelSnapshot> Panels { get; init; }
    public required IEnumerable<UserSnapshot> SharedWith { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}

public static class DashboardEntitySnapshotMapper
{
    public static DashboardSnapshot Save(this DashboardEntity dashboard)
    {
        return new DashboardSnapshot
        {
            DashboardId = dashboard.Id.ToString(),
            Name = dashboard.Name,
            Description = dashboard.Description,
            Owner = dashboard.Owner.Save(),
            Panels = [], // panels does not update with dashboards
            SharedWith = [], // sharedWith does not update with dashboards
            CreatedAt = dashboard.CreatedAt,
            UpdatedAt = dashboard.UpdatedAt
        };
    }

    public static DashboardEntity RestoreFromSnapshot(this DashboardSnapshot snapshot)
    {
        var result = DashboardEntity.Create(
            Guid.Parse(snapshot.DashboardId),
            snapshot.Name,
            snapshot.Description,
            snapshot.Panels.Select(x => x.RestoreFromSnapshot()),
            snapshot.Owner.RestoreFromSnapshot(),
            snapshot.SharedWith.Select(x => x.RestoreFromSnapshot()),
            snapshot.CreatedAt,
            snapshot.UpdatedAt
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSource));
    }
}