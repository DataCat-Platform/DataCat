namespace DataCat.Server.Domain.Identity.Errors;

public sealed class UserError(string code, string message) : BaseError(code, message)
{
    public static readonly UserError NotFound = new("User.NotFound", "The current user could not be found.");
}