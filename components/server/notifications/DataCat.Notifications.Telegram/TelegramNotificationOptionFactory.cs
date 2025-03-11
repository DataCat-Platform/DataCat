namespace DataCat.Notifications.Telegram;

public sealed class TelegramNotificationOptionFactory : INotificationOptionFactory
{
    public NotificationDestination NotificationDestination => NotificationDestination.Telegram;

    public Result<BaseNotificationOption> Create(string settings)
    {
        if (string.IsNullOrWhiteSpace(settings))
        {
            return Result.Fail<BaseNotificationOption>(BaseError.FieldIsNull(settings));
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
            return Result.Fail<BaseNotificationOption>("Invalid JSON format");
        }
    }

    public async Task<Result<INotificationService>> CreateNotificationServiceAsync(
        BaseNotificationOption notificationOption,
        ISecretsProvider secretsProvider,
        CancellationToken cancellationToken = default)
    {
        if (notificationOption is not TelegramNotificationOption telegramNotificationOption)
        {
            return Result.Fail<INotificationService>("Invalid notification option type");
        }

        var telegramToken = await secretsProvider.GetSecretAsync(telegramNotificationOption.TelegramTokenPath, cancellationToken);
        telegramNotificationOption.TelegramToken = telegramToken;
        
        var telegramService = new TelegramNotificationService(telegramNotificationOption);
        return Result.Success<INotificationService>(telegramService);
    }
}