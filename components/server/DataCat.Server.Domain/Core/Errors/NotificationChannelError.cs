namespace DataCat.Server.Domain.Core.Errors;

public sealed class NotificationChannelError(string code, string message) : BaseError(code, message)
{
    public static readonly NotificationChannelError DestinationNotSupported = new("NotificationDestination.NotSupported", "Specified notification destination is not supported.");
    
    public static NotificationChannelError NotFound(string id) => new("NotificationChannel.NotFound", $"NotificationChannel with id {id} is not found.");
    public static NotificationChannelError NotFound(Guid id) => NotFound(id.ToString());
}