namespace DataCat.Storage.Postgres.Snapshots.Users;

public sealed class ExternalRoleMappingSnapshot
{
    public required string ExternalRole { get; init; }
    public required string NamespaceId { get; init; }
    public required int RoleId { get; init; }
}

public static class ExternalRoleMappingSnapshotExtensions
{
    public static ExternalRoleMappingSnapshot Save(this ExternalRoleMapping mapping)
    {
        return new ExternalRoleMappingSnapshot
        {
            ExternalRole = mapping.ExternalRole,
            NamespaceId = mapping.NamespaceId.ToString(),
            RoleId = mapping.Role.Value,
        };
    }
    
    public static ExternalRoleMapping RestoreFromSnapshot(this ExternalRoleMappingSnapshot snapshot) => 
        new(snapshot.ExternalRole, 
            UserRole.FromValue(snapshot.RoleId), 
            namespaceId: Guid.Parse(snapshot.NamespaceId));
}