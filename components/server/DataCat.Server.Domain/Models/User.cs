namespace DataCat.Server.Domain.Models;

public class User
{
    private User(Guid id, string username, string email, UserRole role)
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

    public static Result<User> Create(Guid id, string username, string email, UserRole? role)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result.Fail<User>("Username cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Fail<User>("Email cannot be null or empty");
        }

        if (role is null)
        {
            return Result.Fail<User>("Role cannot be null");
        }

        return Result.Success(new User(id, username, email, role));
    }
}