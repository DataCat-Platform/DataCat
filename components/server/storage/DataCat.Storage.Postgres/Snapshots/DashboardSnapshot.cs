namespace DataCat.Storage.Postgres.Snapshots;

public sealed record DashboardSnapshot
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required UserSnapshot Owner { get; set; }
    public string OwnerId => Owner.UserId;
    public required string NamespaceId { get; init; } 
    public required IList<PanelSnapshot> Panels { get; set; } = [];
    public required IList<UserSnapshot> SharedWith { get; set; } = [];
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required List<Tag> Tags { get; init; }
}

public static class DashboardEntitySnapshotMapper
{
    public static DashboardSnapshot Save(this Dashboard dashboard)
    {
        return new DashboardSnapshot
        {
            Id = dashboard.Id.ToString(),
            Name = dashboard.Name,
            Description = dashboard.Description,
            Owner = dashboard.Owner.Save(),
            Panels = dashboard.Panels.Select(x => x.Save()).ToArray(),
            SharedWith = dashboard.SharedWith.Select(x => x.Save()).ToArray(),
            NamespaceId = dashboard.NamespaceId.ToString(),
            CreatedAt = dashboard.CreatedAt.ToUniversalTime(),
            UpdatedAt = dashboard.UpdatedAt.ToUniversalTime(),
            Tags = dashboard.Tags.ToList()
        };
    }

    public static Dashboard RestoreFromSnapshot(this DashboardSnapshot snapshot)
    {
        var result = Dashboard.Create(
            Guid.Parse(snapshot.Id),
            snapshot.Name,
            snapshot.Description,
            snapshot.Panels.Select(x => x.RestoreFromSnapshot()).ToList(),
            snapshot.Owner.RestoreFromSnapshot(),
            snapshot.SharedWith.Select(x => x.RestoreFromSnapshot()).ToList(),
            namespaceId: Guid.Parse(snapshot.NamespaceId),
            snapshot.CreatedAt.ToUniversalTime(),
            snapshot.UpdatedAt.ToUniversalTime(),
            snapshot.Tags.ToList()
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(DataSource));
    }
}