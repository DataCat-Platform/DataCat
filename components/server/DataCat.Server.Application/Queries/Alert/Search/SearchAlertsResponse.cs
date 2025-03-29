namespace DataCat.Server.Application.Queries.Alert.Search;

public sealed record SearchAlertsResponse
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
    
    public static SearchAlertsResponse ToResponse(AlertEntity alert)
    {
        return new SearchAlertsResponse
        {
            Id = alert.Id,
            Description = alert.Description,
            RawQuery = alert.QueryEntity.RawQuery,
            Status = alert.Status.Name,
            DataSource = alert.QueryEntity.DataSourceEntity.ToResponse(),
            NotificationChannel = alert.NotificationChannelEntity.ToResponse(),
            WaitTimeBeforeAlerting = alert.WaitTimeBeforeAlerting,
            RepeatInterval = alert.RepeatInterval,
            PreviousExecutionTime = alert.PreviousExecution.DateTime,
            NextExecutionTime = alert.NextExecution.DateTime,
        };
    }
}
