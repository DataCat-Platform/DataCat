namespace DataCat.Notifications.Webhook;

public sealed class WebhookNotificationOptionFactory : INotificationOptionFactory
{
    public bool IsResponsibleFor(string notificationOptionName) 
        => string.Compare(notificationOptionName, WebhookConstants.Webhook, 
            StringComparison.InvariantCultureIgnoreCase) == 0;

    public Result<BaseNotificationOption> Create(NotificationDestination? destination, string settings)
    {
        if (string.IsNullOrWhiteSpace(settings))
        {
            return Result.Fail<BaseNotificationOption>(BaseError.FieldIsNull(settings));
        }
        
        if (!WebhookDestinationValidator.IsWebhookDestination(destination))
        {
            Result.Fail<BaseNotificationOption>("Invalid destination type");
        }
        
        try
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(settings);

            if (!jsonElement.TryGetProperty("Url", out var tokenPathElement))
            {
                throw new NotImplementedException("Unknown notification option type");
            }
            
            var url = tokenPathElement.GetString()!;

            return WebhookNotificationOption.Create(destination!, url);
        }
        catch (JsonException)
        {
            return Result.Fail<BaseNotificationOption>("Invalid JSON format");
        }
    }

    public Task<Result<INotificationService>> CreateNotificationServiceAsync(
        BaseNotificationOption notificationOption,
        ISecretsProvider secretsProvider,
        CancellationToken cancellationToken = default)
    {
        if (notificationOption is not WebhookNotificationOption webhookNotificationOption)
        {
            return Task.FromResult(Result.Fail<INotificationService>("Invalid notification option type"));
        }

        var webhookService = new WebhookNotificationService(webhookNotificationOption);
        return Task.FromResult(Result.Success<INotificationService>(webhookService));
    }
}