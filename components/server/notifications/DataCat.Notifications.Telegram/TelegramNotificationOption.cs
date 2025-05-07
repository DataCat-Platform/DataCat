namespace DataCat.Notifications.Telegram;

public sealed record TelegramNotificationOption : BaseNotificationOption
{
    public override string Settings => ToString();
    private TelegramNotificationOption(NotificationDestination destination, string telegramTokenPath, string chatId)
    {
        TelegramTokenPath = telegramTokenPath;
        ChatId = chatId;
        NotificationDestination = destination;
    }

    private string? _telegramToken;
    public string TelegramToken
    {
        get => _telegramToken ?? throw new InvalidOperationException("TelegramToken is not set.");
        set => _telegramToken = value ?? throw new ArgumentNullException(nameof(value), "TelegramToken cannot be null.");
    }
    
    public string TelegramTokenPath { get; init; }
    public string ChatId { get; init; }
    
    public static Result<BaseNotificationOption> Create(NotificationDestination destination, string telegramTokenPath, string chatId)
    {
        var validationList = new List<Result<BaseNotificationOption>>();

        #region Validation

        if (!TelegramDestinationValidator.IsTelegramDestination(destination))
        {
            validationList.Add(Result.Fail<BaseNotificationOption>("Invalid destination type"));
        }
        
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
            : Result.Success<BaseNotificationOption>(new TelegramNotificationOption(destination, telegramTokenPath, chatId));
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(new { TelegramTokenPath, ChatId });
    }
}