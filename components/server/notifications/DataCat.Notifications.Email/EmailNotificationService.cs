namespace DataCat.Notifications.Email;

public sealed class EmailNotificationService(EmailNotificationOption option) : INotificationService
{
    public async Task SendNotificationAsync(Alert alert, CancellationToken token = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("DataCat Alert System", option.DestinationEmail));
        message.To.Add(new MailboxAddress("", option.DestinationEmail));
        message.Subject = $"[ALERT] {alert.Status.Name} – {alert.Id}";

        var bodyText = AlertTemplateRenderer.Render(alert.Template ?? string.Empty, alert);
        
        message.Body = new TextPart("plain")
        {
            Text = bodyText
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(option.SmtpServer, option.Port, useSsl: true, token);
        await client.AuthenticateAsync(option.DestinationEmail, option.Password, token);
        await client.SendAsync(message, token);
        await client.DisconnectAsync(true, token);
    }
}