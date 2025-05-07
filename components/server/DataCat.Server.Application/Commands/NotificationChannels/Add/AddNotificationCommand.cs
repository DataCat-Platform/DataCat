namespace DataCat.Server.Application.Commands.NotificationChannels.Add;

public sealed record AddNotificationCommand : ICommand, IAdminRequest
{
    public required string NotificationChannelGroupName { get; init; } 
    
    public required string DestinationTypeName { get; init; }
    
    public required string Settings { get; init; }
}