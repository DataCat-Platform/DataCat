namespace DataCat.Server.Application.Commands.NotificationChannel.UpdateNotificationQuery;

public sealed record UpdateNotificationCommand : IRequest<Result>, IAdminRequest
{
    public required string NotificationChannelId { get; init; }
    
    public required int DestinationType { get; init; }
    
    public required string Settings { get; init; }
}