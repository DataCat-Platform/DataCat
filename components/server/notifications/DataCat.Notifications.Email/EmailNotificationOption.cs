namespace DataCat.Notifications.Email;

public sealed record EmailNotificationOption : BaseNotificationOption
{
    public override NotificationDestination NotificationDestination => NotificationDestination.Email;
    public override string Settings => ToString();

    private EmailNotificationOption(string destinationEmail, string smtpServer, int port, string passwordPath)
    {
        DestinationEmail = destinationEmail;
        SmtpServer = smtpServer;
        Port = port;
        PasswordPath = passwordPath;
    }

    public string DestinationEmail { get; init; }
    public string SmtpServer { get; init; }
    public int Port { get; init; }
    public string PasswordPath { get; init; }

    public static Result<BaseNotificationOption> Create(
        string destinationEmail, string smtpServer, int? port, string passwordPath)
    {
        var validationList = new List<Result<BaseNotificationOption>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(destinationEmail))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Destination email is required"));
        }

        if (string.IsNullOrWhiteSpace(smtpServer))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("SMTP server is required"));
        }

        if (port is null or <= 0)
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Valid SMTP port is required"));
        }

        if (string.IsNullOrWhiteSpace(passwordPath))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Password path is required"));
        }

        #endregion

        return validationList.Count != 0
            ? validationList.FoldResults()!
            : Result.Success<BaseNotificationOption>(new EmailNotificationOption(destinationEmail, smtpServer, port!.Value, passwordPath));
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(new { DestinationEmail, SmtpServer, Port, PasswordPath });
    }
}