namespace DataCat.Storage.Postgres.Snapshots.Users;

public sealed class AssignedUserRoleSnapshot
{
    public required int RoleId { get; init; }
    public required string NamespaceId { get; init; }
    public required bool IsManual { get; init; }
}

public static class AssignedUserRoleSnapshotExtensions
{
    public static AssignedUserRoleSnapshot Save(this AssignedUserRole role)
    {
        return new AssignedUserRoleSnapshot
        {
            RoleId = role.Role.Value,
            NamespaceId = role.NamespaceId.ToString(),
            IsManual = role.IsManual
        };
    }
    
    public static AssignedUserRole RestoreFromSnapshot(this AssignedUserRoleSnapshot snapshot) => 
        new(UserRole.FromValue(snapshot.RoleId), Guid.Parse(snapshot.NamespaceId), snapshot.IsManual);
}