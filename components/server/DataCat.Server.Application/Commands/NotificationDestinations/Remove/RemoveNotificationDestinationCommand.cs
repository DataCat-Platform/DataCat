namespace DataCat.Server.Application.Commands.NotificationDestinations.Remove;

public sealed record RemoveNotificationDestinationCommand(string Name) : ICommand, IAdminRequest;
