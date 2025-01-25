namespace DataCat.Server.Domain.Identity;

public class AuthToken
{
    private AuthToken(
        Guid id,
        string token,
        Guid userId,
        DateTime expiration,
        DateTime createdAt)
    {
        Id = id;
        Token = token;
        UserId = userId;
        Expiration = expiration;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public string Token { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime Expiration { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Result<AuthToken> Create(
        Guid id,
        string token,
        Guid userId,
        DateTime expiration,
        DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail<AuthToken>("Token cannot be null or empty");
        }

        if (userId == Guid.Empty)
        {
            return Result.Fail<AuthToken>("UserId cannot be an empty GUID");
        }

        if (expiration <= createdAt)
        {
            return Result.Fail<AuthToken>("Expiration must be after CreatedAt");
        }

        return Result.Success(new AuthToken(id, token, userId, expiration, createdAt));
    }
}