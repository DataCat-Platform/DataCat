namespace DataCat.Server.Application.Commands.NotificationDestinations.Add;

public sealed record AddNotificationDestinationCommand(string Name) : IRequest<Result<int>>, IAdminRequest;
