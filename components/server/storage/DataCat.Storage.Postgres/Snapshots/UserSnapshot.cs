namespace DataCat.Storage.Postgres.Snapshots;

public sealed class UserSnapshot
{
    public required string UserId { get; init; }
    public required string IdentityId { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime? UpdatedAt { get; init; }
    public required List<AssignedUserRoleSnapshot> Roles { get; set; } = [];
    public required List<AssignedUserPermissionSnapshot> Permissions { get; set; } = [];
}

public sealed class AssignedUserRoleSnapshot
{
    public required int RoleId { get; init; }
    public required string NamespaceId { get; init; }
    public required bool IsManual { get; init; }
} 

public sealed class AssignedUserPermissionSnapshot
{
    public required int PermissionId { get; init; }
    public required string NamespaceId { get; init; }
    public required bool IsManual { get; init; }
}

public static class UserSnapshotExtensions 
{
    public static UserSnapshot Save(this UserEntity user)
    {
        return new UserSnapshot
        {
            UserId = user.Id.ToString(),
            IdentityId = user.IdentityId,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Roles = user.Roles.Select(r => r.Save()).ToList(),
            Permissions = user.Permissions.Select(p => p.Save()).ToList()
        };
    }

    private static AssignedUserRoleSnapshot Save(this AssignedUserRole role)
    {
        return new AssignedUserRoleSnapshot
        {
            RoleId = role.Role.Value,
            NamespaceId = role.NamespaceId.ToString(),
            IsManual = role.IsManual
        };
    }

    private static AssignedUserPermissionSnapshot Save(this AssignedUserPermissions permission)
    {
        return new AssignedUserPermissionSnapshot
        {
            PermissionId = permission.Permission.Value,
            NamespaceId = permission.NamespaceId.ToString(),
            IsManual = permission.IsManual
        };
    }

    public static UserEntity RestoreFromSnapshot(this UserSnapshot snapshot)
    {
        var result = UserEntity.Create(
            Guid.Parse(snapshot.UserId),
            snapshot.IdentityId,
            snapshot.Email,
            snapshot.Name,
            snapshot.CreatedAt,
            snapshot.UpdatedAt,
            snapshot.Roles.Select(r => r.RestoreFromSnapshot()),
            snapshot.Permissions.Select(p => p.RestoreFromSnapshot()));
        
        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(UserEntity));
    }

    private static AssignedUserPermissions RestoreFromSnapshot(this AssignedUserPermissionSnapshot snapshot) => 
        new(UserPermission.FromValue(snapshot.PermissionId), Guid.Parse(snapshot.NamespaceId), snapshot.IsManual);

    private static AssignedUserRole RestoreFromSnapshot(this AssignedUserRoleSnapshot snapshot) => 
        new(UserRole.FromValue(snapshot.RoleId), Guid.Parse(snapshot.NamespaceId), snapshot.IsManual);
}