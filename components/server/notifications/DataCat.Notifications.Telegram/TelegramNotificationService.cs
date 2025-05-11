namespace DataCat.Notifications.Telegram;

public sealed class TelegramNotificationService(TelegramNotificationOption option) : INotificationService
{
    public async Task SendNotificationAsync(Alert alert, CancellationToken token = default)
    {
        Console.WriteLine($"[TelegramNotificationService] Sending notification, {option.Settings}");
        var bot = new TelegramBotClient(option.TelegramToken);
        
        var message = AlertTemplateRenderer.Render(alert.Template ?? string.Empty, alert);

        await bot.SendMessage(
            chatId: option.ChatId,
            text: EscapeMarkdown(message),
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