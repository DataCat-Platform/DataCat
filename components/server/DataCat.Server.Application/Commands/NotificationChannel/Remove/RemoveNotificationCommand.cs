namespace DataCat.Server.Application.Commands.NotificationChannel.Remove;

public sealed record RemoveNotificationCommand(string NotificationId) : IRequest<Result>, IAdminRequest;
