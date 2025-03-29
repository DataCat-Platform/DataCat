namespace DataCat.Storage.Postgres.Snapshots;

public sealed class UserSnapshot
{
    public required string UserId { get; init; }
}

public static class UserEntitySnapshotMapper 
{
    public static UserSnapshot Save(this UserEntity UserEntity)
    {
        return new UserSnapshot
        {
            UserId = UserEntity.Id.ToString(),
        };
    }

    public static UserEntity RestoreFromSnapshot(this UserSnapshot snapshot)
    {
        var result = UserEntity.Create(Guid.Parse(snapshot.UserId));

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(UserEntity));
    }
}