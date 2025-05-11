namespace DataCat.Notifications.Webhook;

public static class WebhookDestinationValidator
{
    public static bool IsWebhookDestination(NotificationDestination? destination)
    {
        if (destination is null)
        {
            return false;
        }

        return string.Compare(destination.Name, WebhookConstants.Webhook, 
            StringComparison.InvariantCultureIgnoreCase) == 0;
    }
}