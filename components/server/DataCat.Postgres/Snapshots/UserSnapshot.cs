namespace DataCat.Server.Postgres.Snapshots;

public class UserSnapshot
{
    public const string UserTable = "datacat_users";

    public required string UserId { get; init; }
    public required string UserName { get; init; }
    public required string UserEmail { get; init; }
    public required int UserRole { get; init; }
}

public static class UserEntitySnapshotMapper 
{
    public static UserSnapshot ReadUser(this DbDataReader reader)
    {
        return new UserSnapshot
        {
            UserId = reader.GetString(reader.GetOrdinal(Public.Users.UserId)),
            UserName = reader.GetString(reader.GetOrdinal(Public.Users.UserName)),
            UserEmail = reader.GetString(reader.GetOrdinal(Public.Users.UserEmail)),
            UserRole = reader.GetInt32(reader.GetOrdinal(Public.Users.UserRole))
        };
    }
    
    public static UserSnapshot Save(this UserEntity UserEntity)
    {
        return new UserSnapshot
        {
            UserId = UserEntity.Id.ToString(),
            UserName = UserEntity.Username,
            UserEmail = UserEntity.Email,
            UserRole = UserEntity.Role.Value
        };
    }

    public static UserEntity RestoreFromSnapshot(this UserSnapshot snapshot)
    {
        var result = UserEntity.Create(
            Guid.Parse(snapshot.UserId),
            snapshot.UserName,
            snapshot.UserEmail,
            UserRole.FromValue(snapshot.UserRole));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(UserEntity));
    }
}