namespace DataCat.Server.Application.Commands.NotificationChannels.UpdateNotificationQuery;

public sealed record UpdateNotificationCommand : IRequest<Result>, IAdminRequest
{
    public required string NotificationChannelId { get; init; }
    
    public required string DestinationTypeName { get; init; }
    
    public required string Settings { get; init; }
}