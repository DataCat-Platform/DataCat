namespace DataCat.Server.Application.Auth;

public interface IJwtService
{
    Task<Result<string>> GetUserAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<Result<string>> GetServerAccessTokenAsync(CancellationToken cancellationToken = default);
}