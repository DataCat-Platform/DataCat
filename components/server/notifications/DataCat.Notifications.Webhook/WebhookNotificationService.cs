namespace DataCat.Notifications.Webhook;

public sealed class WebhookNotificationService(WebhookNotificationOption option) : INotificationService
{
    private static readonly HttpClient _httpClient = new();

    public async Task SendNotificationAsync(Alert alert, CancellationToken token = default)
    {
        Console.WriteLine($"[WebhookNotificationService] Sending notification, {option.Settings}");

        var message = AlertTemplateRenderer.Render(alert.Template ?? string.Empty, alert);

        var json = JsonSerializer.Serialize(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(option.Url, content, token);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[WebhookNotificationService] Failed to send webhook: {response.StatusCode} - {await response.Content.ReadAsStringAsync(token)}");
        }
    }
}