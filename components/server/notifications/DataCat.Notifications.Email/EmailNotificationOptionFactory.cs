namespace DataCat.Notifications.Email;

public sealed class EmailNotificationOptionFactory : INotificationOptionFactory
{
    public bool IsResponsibleFor(string notificationOptionName) 
        => string.Compare(notificationOptionName, EmailConstants.Email, 
            StringComparison.InvariantCultureIgnoreCase) == 0;

    public Result<BaseNotificationOption> Create(NotificationDestination? destination, string settings)
    {
        if (string.IsNullOrWhiteSpace(settings))
        {
            return Result.Fail<BaseNotificationOption>(BaseError.FieldIsNull(settings));
        }
        
        if (!EmailDestinationValidator.IsEmailDestination(destination))
        {
            Result.Fail<BaseNotificationOption>("Invalid destination type");
        }
        
        try
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(settings);

            if (!jsonElement.TryGetProperty("DestinationEmail", out var destinationEmailElement)
                || !jsonElement.TryGetProperty("SmtpServer", out var smtpServerElement)
                || !jsonElement.TryGetProperty("Port", out var portElement)
                || !jsonElement.TryGetProperty("PasswordPath", out var passwordPathElement))
            {
                throw new NotImplementedException("Unknown notification option type");
            }
            
            var destinationEmail = destinationEmailElement.GetString()!;
            var smtpServer = smtpServerElement.GetString()!;
            var port = portElement.GetInt32();
            var passwordPath = passwordPathElement.GetString()!;

            return EmailNotificationOption.Create(destination, destinationEmail, smtpServer, port, passwordPath);
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
        await Task.CompletedTask;
        return notificationOption is not EmailNotificationOption emailNotificationOption 
            ? Result.Fail<INotificationService>("Unknown notification option type") 
            : Result.Success<INotificationService>(new EmailNotificationService(emailNotificationOption));
    }
}