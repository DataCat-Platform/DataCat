namespace DataCat.Notifications.Email;

public sealed record EmailNotificationOption : BaseNotificationOption
{
    public new string Settings => ToString();

    private EmailNotificationOption(
        NotificationDestination destination,
        string destinationEmail,
        string smtpServer, 
        int port, 
        string passwordPath)
    {
        DestinationEmail = destinationEmail;
        SmtpServer = smtpServer;
        Port = port;
        PasswordPath = passwordPath;
        NotificationDestination = destination;
    }

    public string DestinationEmail { get; private set; }
    public string SmtpServer { get; private set; }
    public int Port { get; private set; }
    public string PasswordPath { get; private set; }

    public static Result<BaseNotificationOption> Create(
        NotificationDestination? destination,
        string destinationEmail, string smtpServer, int? port, string passwordPath)
    {
        var validationList = new List<Result<BaseNotificationOption>>();

        #region Validation

        if (!EmailDestinationValidator.IsEmailDestination(destination))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Invalid destination type"));
        }
        
        if (string.IsNullOrWhiteSpace(destinationEmail))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>(BaseError.FieldIsNull(nameof(destinationEmail))));
        }

        if (string.IsNullOrWhiteSpace(smtpServer))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>(BaseError.FieldIsNull(nameof(smtpServer))));
        }

        if (port is null or <= 0)
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Invalid port number"));
        }

        if (string.IsNullOrWhiteSpace(passwordPath))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>(BaseError.FieldIsNull(nameof(passwordPath))));
        }

        #endregion

        return validationList.Count != 0
            ? validationList.FoldResults()!
            : Result.Success<BaseNotificationOption>(new EmailNotificationOption(
                destination!,
                destinationEmail,
                smtpServer,
                port!.Value,
                passwordPath));
    }

    public override string ToString()
    {
        const string password = "***";
        return JsonSerializer.Serialize(new { DestinationEmail, SmtpServer, Port, password });
    }
}