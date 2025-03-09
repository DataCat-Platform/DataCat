namespace DataCat.Notifications.Telegram;

public sealed class TelegramNotificationOptionFactory : INotificationOptionFactory
{
    public NotificationDestination NotificationDestination => NotificationDestination.Telegram;

    public Result<BaseNotificationOption> Create(string settings)
    {
        if (string.IsNullOrWhiteSpace(settings))
        {
            throw new ArgumentNullException(nameof(settings));
        }
        
        try
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(settings);

            if (!jsonElement.TryGetProperty("TelegramTokenPath", out var tokenPathElement)
                || !jsonElement.TryGetProperty("ChatId", out var chatIdElement))
            {
                throw new NotImplementedException("Unknown notification option type");
            }
            
            var telegramTokenPath = tokenPathElement.GetString()!;
            var chatId = chatIdElement.GetString()!;

            return TelegramNotificationOption.Create(telegramTokenPath, chatId);
        }
        catch (JsonException)
        {
            throw new InvalidOperationException("Invalid JSON format");
        }
    }
}