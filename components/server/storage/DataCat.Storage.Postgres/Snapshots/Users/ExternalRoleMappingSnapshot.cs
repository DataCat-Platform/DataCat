namespace DataCat.Storage.Postgres.Snapshots.Users;

public sealed class ExternalRoleMappingSnapshot
{
    public required string ExternalRole { get; init; }
    public required string NamespaceId { get; init; }
    public required int RoleId { get; init; }
}

public static class ExternalRoleMappingSnapshotExtensions
{
    public static ExternalRoleMappingSnapshot Save(this ExternalRoleMappingValue mappingValue)
    {
        return new ExternalRoleMappingSnapshot
        {
            ExternalRole = mappingValue.ExternalRole,
            NamespaceId = mappingValue.NamespaceId.ToString(),
            RoleId = mappingValue.Role.Value,
        };
    }
    
    public static ExternalRoleMappingValue RestoreFromSnapshot(this ExternalRoleMappingSnapshot snapshot) => 
        new(snapshot.ExternalRole, 
            UserRole.FromValue(snapshot.RoleId), 
            namespaceId: Guid.Parse(snapshot.NamespaceId));
}