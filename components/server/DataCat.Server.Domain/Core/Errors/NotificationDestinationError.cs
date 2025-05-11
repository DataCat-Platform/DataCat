namespace DataCat.Server.Domain.Core.Errors;

public sealed class NotificationDestinationError(string code, string message) : BaseError(code, message)
{
    public static NotificationDestinationError NotFound(string name) => new("NotificationDestination.NotFound", $"NotificationDestination with name {name} is not found.");
}