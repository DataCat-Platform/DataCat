namespace DataCat.Notifications.Telegram;

public static class TelegramDestinationValidator
{
    public static bool IsTelegramDestination(NotificationDestination? destination)
    {
        if (destination is null)
        {
            return false;
        }

        return string.Compare(destination.Name, TelegramConstants.Telegram, 
            StringComparison.InvariantCultureIgnoreCase) == 0;
    }
}