namespace DataCat.Server.Postgres.Snapshots;

public class UserSnapshot
{
    public const string UserTable = "datacat_users";

    public required string UserId { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
}

public static class UserEntitySnapshotMapper 
{
    public static UserSnapshot Save(this UserEntity UserEntity)
    {
        return new UserSnapshot
        {
            UserId = UserEntity.Id.ToString(),
            Name = UserEntity.Username,
            Email = UserEntity.Email,
            Role = UserEntity.Role.Name
        };
    }

    public static UserEntity RestoreFromSnapshot(this UserSnapshot snapshot)
    {
        var result = UserEntity.Create(
            Guid.Parse(snapshot.UserId),
            snapshot.Name,
            snapshot.Email,
            UserRole.FromName(snapshot.Role));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(UserEntity));
    }
}