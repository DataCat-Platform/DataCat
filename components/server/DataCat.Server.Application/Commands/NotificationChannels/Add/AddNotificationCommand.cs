namespace DataCat.Server.Application.Commands.NotificationChannels.Add;

public sealed record AddNotificationCommand : ICommand<Guid>, IAdminRequest
{
    public required string DestinationTypeName { get; init; }
    
    public required string Settings { get; init; }
}