namespace DataCat.Server.Domain.Identity.Errors;

public sealed class UserError(string code, string message) : BaseError(code, message)
{
    public static readonly UserError NotFound = new("User.NotFound", "The current user could not be found");
    public static readonly UserError InvalidUserRole = new("User.Role", "Current role is invalid");
    public static readonly UserError InvalidCredentials = new("User.Auth", "Invalid user name or password");
}