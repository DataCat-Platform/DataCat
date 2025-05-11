namespace DataCat.Server.Application.Auth;

public interface IOidcRedirectService
{
    string GenerateRedirectUrl();
}