namespace DataCat.Storage.Postgres.Snapshots.Users;

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

public static class UserSnapshotExtensions 
{
    public static UserSnapshot Save(this User user)
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

    public static User RestoreFromSnapshot(this UserSnapshot snapshot)
    {
        var result = User.Create(
            Guid.Parse(snapshot.UserId),
            snapshot.IdentityId,
            snapshot.Email,
            snapshot.Name,
            snapshot.CreatedAt,
            snapshot.UpdatedAt,
            snapshot.Roles.Select(r => r.RestoreFromSnapshot()),
            snapshot.Permissions.Select(p => p.RestoreFromSnapshot()));
        
        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(User));
    }
}