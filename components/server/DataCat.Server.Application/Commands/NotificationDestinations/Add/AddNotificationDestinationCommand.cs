namespace DataCat.Server.Application.Commands.NotificationDestinations.Add;

public sealed record AddNotificationDestinationCommand(string Name) : ICommand<int>, IAdminRequest;
