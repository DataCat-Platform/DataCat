namespace DataCat.Notifications.Email;

public sealed class EmailNotificationOptionFactory : INotificationOptionFactory
{
    public NotificationDestination NotificationDestination => NotificationDestination.Email;

    public Result<BaseNotificationOption> Create(string settings)
    {
        if (string.IsNullOrWhiteSpace(settings))
        {
            throw new ArgumentNullException(nameof(settings));
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

            return EmailNotificationOption.Create(destinationEmail, smtpServer, port, passwordPath);
        }
        catch (JsonException)
        {
            throw new InvalidOperationException("Invalid JSON format");
        }
    }
}