namespace DataCat.Server.Postgres.Snapshots;

public class DashboardSnapshot
{
    public const string DashboardTable = "dashboards";
    public const string DashboardOwnerId = "dashboard_owner_id";

    public required string DashboardId { get; set; }
    public required string DashboardName { get; set; }
    public required string? DashboardDescription { get; set; }
    public required UserSnapshot Owner { get; set; }
    public string OwnerId => Owner.UserId;
    public required IList<PanelSnapshot> Panels { get; set; }
    public required IList<UserSnapshot> SharedWith { get; set; }
    public required DateTime DashboardCreatedAt { get; set; }
    public required DateTime DashboardUpdatedAt { get; set; }
}

public static class DashboardEntitySnapshotMapper
{
    public static DashboardSnapshot Save(this DashboardEntity dashboard)
    {
        return new DashboardSnapshot
        {
            DashboardId = dashboard.Id.ToString(),
            DashboardName = dashboard.Name,
            DashboardDescription = dashboard.Description,
            Owner = dashboard.Owner.Save(),
            Panels = dashboard.Panels.Select(x => x.Save()).ToArray(),
            SharedWith = dashboard.SharedWith.Select(x => x.Save()).ToArray(),
            DashboardCreatedAt = dashboard.CreatedAt,
            DashboardUpdatedAt = dashboard.UpdatedAt
        };
    }

    public static DashboardEntity RestoreFromSnapshot(this DashboardSnapshot snapshot)
    {
        var result = DashboardEntity.Create(
            Guid.Parse(snapshot.DashboardId),
            snapshot.DashboardName,
            snapshot.DashboardDescription,
            snapshot.Panels?.Select(x => x.RestoreFromSnapshot())?.ToList(),
            snapshot.Owner.RestoreFromSnapshot(),
            snapshot.SharedWith?.Select(x => x.RestoreFromSnapshot())?.ToList(),
            snapshot.DashboardCreatedAt,
            snapshot.DashboardUpdatedAt
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSourceEntity));
    }
}