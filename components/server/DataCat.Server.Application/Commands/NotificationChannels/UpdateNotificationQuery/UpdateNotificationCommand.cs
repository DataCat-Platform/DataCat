namespace DataCat.Server.Application.Commands.NotificationChannels.UpdateNotificationQuery;

public sealed record UpdateNotificationCommand : ICommand, IAdminRequest
{
    public required int NotificationChannelId { get; init; }
    
    public required string DestinationTypeName { get; init; }
    
    public required string Settings { get; init; }
}