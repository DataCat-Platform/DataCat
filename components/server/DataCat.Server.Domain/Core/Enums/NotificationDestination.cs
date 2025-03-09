namespace DataCat.Server.Domain.Core.Enums;

public abstract class NotificationDestination(string name, int value)
    : SmartEnum<NotificationDestination, int>(name, value)
{
    public static readonly NotificationDestination Telegram = new TelegramNotification();
    public static readonly NotificationDestination Email = new EmailNotification();
    public static readonly NotificationDestination ThirdParty = new ThirdPartyNotification();

    private sealed class TelegramNotification() : NotificationDestination("Telegram", 1);
    
    private sealed class EmailNotification() : NotificationDestination("Email", 2);
    
    private sealed class ThirdPartyNotification() : NotificationDestination("ThirdParty", 3);
}