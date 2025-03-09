namespace DataCat.Notifications.Telegram;

public sealed record TelegramNotificationOption : BaseNotificationOption
{
    public override NotificationDestination NotificationDestination => NotificationDestination.Telegram;
    public override string Settings => ToString();

    private TelegramNotificationOption(string telegramTokenPath, string chatId)
    {
        TelegramTokenPath = telegramTokenPath;
        ChatId = chatId;
    }
    
    public string TelegramTokenPath { get; init; }
    public string ChatId { get; init; }

    public static Result<BaseNotificationOption> Create(string telegramTokenPath, string chatId)
    {
        var validationList = new List<Result<BaseNotificationOption>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(telegramTokenPath))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Telegram token path is required"));
        }
        
        if (string.IsNullOrWhiteSpace(chatId))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Chat id is required"));
        }

        #endregion
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success<BaseNotificationOption>(new TelegramNotificationOption(telegramTokenPath, chatId));
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(new { TelegramTokenPath, ChatId});
    }
}