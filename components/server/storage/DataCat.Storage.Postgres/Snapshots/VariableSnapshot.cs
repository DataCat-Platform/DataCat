namespace DataCat.Storage.Postgres.Snapshots;

public class VariableSnapshot
{
    public required string Id { get; init; }
    public required string Placeholder { get; init; }
    public required string Value { get; init; }
    public required string NamespaceId { get; init; }
    public required string? DashboardId { get; init; }
}

public static class VariableSnapshotExtensions
{
    public static VariableSnapshot Save(this Variable variable)
    {
        return new VariableSnapshot
        {
            Id = variable.Id.ToString(),
            Placeholder = variable.Placeholder,
            Value = variable.Value,
            NamespaceId = variable.NamespaceId.ToString(),
            DashboardId = variable.DashboardId?.ToString()
        };
    }

    public static Variable RestoreFromSnapshot(this VariableSnapshot snapshot)
    {
        var result = Variable.Create(
            Guid.Parse(snapshot.Id),
            snapshot.Placeholder,
            snapshot.Value,
            Guid.Parse(snapshot.NamespaceId),
            snapshot.DashboardId is null ? null : Guid.Parse(snapshot.DashboardId));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(Variable));
    }
}