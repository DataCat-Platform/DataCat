namespace DataCat.Server.PDK;

public interface INotificationPlugin
{
    string Name { get; }
    string Description { get; }
    Task SendAlertAsync(AlertMessage alert, CancellationToken token);
}

public sealed record AlertMessage
{
    public required string Title { get; init; }
    public required string Message { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}