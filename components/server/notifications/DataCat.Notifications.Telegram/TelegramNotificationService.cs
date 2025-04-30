namespace DataCat.Notifications.Telegram;

public sealed class TelegramNotificationService(TelegramNotificationOption option) : INotificationService
{
    public async Task SendNotificationAsync(Alert alert, CancellationToken token = default)
    {
        Console.WriteLine($"[TelegramNotificationService] Sending notification, {option.Settings}");
        var bot = new TelegramBotClient(option.TelegramToken);
        
        var message = $"""
                           🔔 *Alert Notification* 🔔
                       
                           *ID:* `{alert.Id}`
                           *Status:* {alert.Status.Name}
                           *Description:* {EscapeMarkdown(alert.Description ?? "No description")}
                           *Query:* `{EscapeMarkdown(alert.Query.RawQuery)}`
                           *Previous Execution:* `{alert.PreviousExecution.DateTime}`
                           *Next Execution:* `{alert.NextExecution.DateTime}`
                           *Repeat Interval:* `{alert.RepeatInterval}`
                           *Wait Time Before Alerting:* `{alert.WaitTimeBeforeAlerting}`
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