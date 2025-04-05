namespace DataCat.Storage.Postgres.Snapshots.Users;

public sealed class AssignedUserPermissionSnapshot
{
    public required int PermissionId { get; init; }
    public required string NamespaceId { get; init; }
    public required bool IsManual { get; init; }
}

public static class AssignedUserPermissionSnapshotExtensions
{
    public static AssignedUserPermissionSnapshot Save(this AssignedUserPermissions permission)
    {
        return new AssignedUserPermissionSnapshot
        {
            PermissionId = permission.Permission.Value,
            NamespaceId = permission.NamespaceId.ToString(),
            IsManual = permission.IsManual
        };
    }
    
    public static AssignedUserPermissions RestoreFromSnapshot(this AssignedUserPermissionSnapshot snapshot) => 
        new(UserPermission.FromValue(snapshot.PermissionId), Guid.Parse(snapshot.NamespaceId), snapshot.IsManual);
}