namespace DataCat.Server.Domain.Core.Enums;

public abstract class NotificationDestination(string name, int value)
    : SmartEnum<NotificationDestination, int>(name, value)
{
    public static readonly NotificationDestination Telegram = new TelegramNotification();
    public static readonly NotificationDestination Email = new EmailNotification();

    private sealed class TelegramNotification() : NotificationDestination("Telegram", 1);
    
    private sealed class EmailNotification() : NotificationDestination("Email", 2);
}