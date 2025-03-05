namespace DataCat.Server.HttpModels.Responses.Alert;

public class AlertResponse
{
    public required Guid Id { get; init; }

    public required string? Description { get; init; }
        
    public required string RawQuery { get; init; }
    
    public required string Status { get; init; }
    
    public required DataSourceResponse DataSource { get; init; }
    
    public required NotificationChannelResponse NotificationChannel { get; init; }
    
    public required DateTime PreviousExecutionTime { get; init; }
    
    public required DateTime NextExecutionTime { get; init; }
    
    public required TimeSpan WaitTimeBeforeAlerting { get; init; }
    
    public required TimeSpan RepeatInterval { get; init; }
}