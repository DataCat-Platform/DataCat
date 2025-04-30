namespace DataCat.Storage.Postgres.Snapshots;

public sealed record NamespaceSnapshot
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required List<string> DashboardIds { get; init; } = [];
}

public static class NamespaceSnapshotExtensions
{
    public static NamespaceSnapshot Save(this Namespace @namespace)
    {
        return new NamespaceSnapshot
        {
            Id = @namespace.Id.ToString(),
            Name = @namespace.Name,
            DashboardIds = @namespace.DashboardIds.Select(x => x.ToString()).ToList()
        };
    }

    public static Namespace RestoreFromSnapshot(this NamespaceSnapshot snapshot)
    {
        return Namespace.Create(
            Guid.Parse(snapshot.Id), 
            snapshot.Name, 
            snapshot.DashboardIds.Select(Guid.Parse).ToList()).Value;
    }
}