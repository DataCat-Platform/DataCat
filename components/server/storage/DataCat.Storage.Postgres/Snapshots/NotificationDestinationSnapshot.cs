namespace DataCat.Storage.Postgres.Snapshots;

public sealed record NotificationDestinationSnapshot
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public static class NotificationDestinationSnapshotExtensions
{
    public static NotificationDestinationSnapshot Save(this NotificationDestination destination)
    {
        return new NotificationDestinationSnapshot
        {
            Id = destination.Id,
            Name = destination.Name
        };
    }

    public static NotificationDestination RestoreFromSnapshot(this NotificationDestinationSnapshot snapshot)
    {
        var destination = NotificationDestination.Create(snapshot.Name, snapshot.Id).Value;
        return destination;
    }
}