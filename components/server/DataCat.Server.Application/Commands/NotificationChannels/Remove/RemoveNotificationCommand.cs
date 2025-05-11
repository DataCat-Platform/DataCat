namespace DataCat.Server.Application.Commands.NotificationChannels.Remove;

public sealed record RemoveNotificationCommand(int NotificationId) : ICommand, IAdminRequest;
