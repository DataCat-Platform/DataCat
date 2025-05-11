namespace DataCat.Notifications.Webhook;

public sealed record WebhookNotificationOption : BaseNotificationOption
{
    public override string Settings => ToString();
    
    private WebhookNotificationOption(NotificationDestination destination, string url)
    {
        Url = url;
        NotificationDestination = destination;
    }
    
    public string Url { get; }
    
    public static Result<BaseNotificationOption> Create(NotificationDestination destination, string url)
    {
        var validationList = new List<Result<BaseNotificationOption>>();

        #region Validation

        if (!WebhookDestinationValidator.IsWebhookDestination(destination))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Invalid destination type"));
        }
        
        if (string.IsNullOrWhiteSpace(url))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Url is required for sending alerts"));
        }

        #endregion
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success<BaseNotificationOption>(new WebhookNotificationOption(destination, url));
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(new { Url });
    }
}