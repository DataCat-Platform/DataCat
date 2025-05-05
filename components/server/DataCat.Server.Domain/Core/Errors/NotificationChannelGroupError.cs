namespace DataCat.Server.Domain.Core.Errors;

public sealed class NotificationChannelGroupError(string code, string message) : BaseError(code, message)
{
    public static NotificationChannelGroupError NotFound(string name) => new("NotificationChannelGroup.NotFound", $"NotificationChannelGroup with name {name} is not found.");
}