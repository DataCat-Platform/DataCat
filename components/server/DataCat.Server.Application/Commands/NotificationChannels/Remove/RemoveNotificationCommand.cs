namespace DataCat.Server.Application.Commands.NotificationChannels.Remove;

public sealed record RemoveNotificationCommand(string NotificationId) : ICommand, IAdminRequest;
