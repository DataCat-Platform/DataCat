namespace DataCat.Server.Domain.Identity;

public class UserEntity
{
    private UserEntity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }

    public static Result<UserEntity> Create(Guid id)
    {
        return Result.Success(new UserEntity(id));
    }
}