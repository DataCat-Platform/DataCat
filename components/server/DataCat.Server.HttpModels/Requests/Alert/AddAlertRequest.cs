namespace DataCat.Server.HttpModels.Requests.Alert;

public class AddAlertRequest
{
    public string? Description { get; init; }
    
    public required string RawQuery  { get; init; }
    
    public required string DataSourceId { get; init; }

    public required string NotificationChannelId { get; init; }
    
    public required TimeSpan WaitTimeBeforeAlerting { get; init; }
    
    public required TimeSpan RepeatInterval { get; init; }
}