namespace DataCat.Server.Application.Commands.NotificationChannel.Add;

public sealed record AddNotificationCommand : IRequest<Result<Guid>>
{
    public required int DestinationType { get; init; }
    
    public required string Settings { get; init; }
}