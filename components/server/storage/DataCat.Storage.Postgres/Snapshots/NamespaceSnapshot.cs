namespace DataCat.Storage.Postgres.Snapshots;

public sealed record NamespaceSnapshot
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required List<string> DashboardIds { get; init; } = [];
}

public static class NamespaceSnapshotExtensions
{
    public static NamespaceSnapshot Save(this NamespaceEntity namespaceEntity)
    {
        return new NamespaceSnapshot
        {
            Id = namespaceEntity.Id.ToString(),
            Name = namespaceEntity.Name,
            DashboardIds = namespaceEntity.DashboardIds.Select(x => x.ToString()).ToList()
        };
    }

    public static NamespaceEntity RestoreFromSnapshot(this NamespaceSnapshot snapshot)
    {
        return NamespaceEntity.Create(
            Guid.Parse(snapshot.Id), 
            snapshot.Name, 
            snapshot.DashboardIds.Select(Guid.Parse).ToList()).Value;
    }
}