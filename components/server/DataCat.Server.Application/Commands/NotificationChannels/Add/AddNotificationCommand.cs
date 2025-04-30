namespace DataCat.Server.Application.Commands.NotificationChannels.Add;

public sealed record AddNotificationCommand : IRequest<Result<Guid>>, IAdminRequest
{
    public required string DestinationTypeName { get; init; }
    
    public required string Settings { get; init; }
}