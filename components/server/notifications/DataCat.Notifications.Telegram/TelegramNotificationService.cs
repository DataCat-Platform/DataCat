namespace DataCat.Notifications.Telegram;

public sealed class TelegramNotificationService(TelegramNotificationOption option) : INotificationService
{
    public async Task SendNotificationAsync(AlertEntity alertEntity, CancellationToken token = default)
    {
        Console.WriteLine($"[TelegramNotificationService] Sending notification, {option.Settings}");
        var bot = new TelegramBotClient(option.TelegramToken);
        
        var message = $"""
                           🔔 *Alert Notification* 🔔
                       
                           *ID:* `{alertEntity.Id}`
                           *Status:* {alertEntity.Status.Name}
                           *Description:* {EscapeMarkdown(alertEntity.Description ?? "No description")}
                           *Query:* `{EscapeMarkdown(alertEntity.QueryEntity.RawQuery)}`
                           *Previous Execution:* `{alertEntity.PreviousExecution.DateTime}`
                           *Next Execution:* `{alertEntity.NextExecution.DateTime}`
                           *Repeat Interval:* `{alertEntity.RepeatInterval}`
                           *Wait Time Before Alerting:* `{alertEntity.WaitTimeBeforeAlerting}`
                       """;

        await bot.SendMessage(
            chatId: option.ChatId,
            text: message,
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: token
        );
    }
    
    private static string EscapeMarkdown(string text)
    {
        return text
            .Replace("_", "\\_")
            .Replace("*", "\\*")
            .Replace("[", "\\[")
            .Replace("]", "\\]")
            .Replace("(", "\\(")
            .Replace(")", "\\)")
            .Replace("~", "\\~")
            .Replace("`", "\\`")
            .Replace(">", "\\>")
            .Replace("#", "\\#")
            .Replace("+", "\\+")
            .Replace("-", "\\-")
            .Replace("=", "\\=")
            .Replace("|", "\\|")
            .Replace("{", "\\{")
            .Replace("}", "\\}")
            .Replace(".", "\\.")
            .Replace("!", "\\!");
    }
}