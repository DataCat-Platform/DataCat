namespace DataCat.Server.Domain.Identity;

public class UserEntity
{
    private UserEntity(Guid id, string username, string email, UserRole role)
    {
        Id = id;
        Username = username;
        Email = email;
        Role = role;
    }

    public Guid Id { get; private set; }

    public string Username { get; private set; }

    public string Email { get; private set; }

    public UserRole Role { get; private set; }

    public static Result<UserEntity> Create(Guid id, string username, string email, UserRole? role)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result.Fail<UserEntity>("Username cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Fail<UserEntity>("Email cannot be null or empty");
        }

        if (role is null)
        {
            return Result.Fail<UserEntity>("Role cannot be null");
        }

        return Result.Success(new UserEntity(id, username, email, role));
    }
}