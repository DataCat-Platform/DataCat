namespace DataCat.Server.Application.Queries.Alerts.Get;

public sealed record GetAlertResponse
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

public static class GetAlertResponseExtensions
{
    public static GetAlertResponse ToResponse(this Alert alert)
    {
        return new GetAlertResponse
        {
            Id = alert.Id,
            Description = alert.Description,
            RawQuery = alert.Query.RawQuery,
            Status = alert.Status.Name,
            DataSource = alert.Query.DataSource.ToResponse(),
            NotificationChannel = alert.NotificationChannel.ToResponse(),
            WaitTimeBeforeAlerting = alert.WaitTimeBeforeAlerting,
            RepeatInterval = alert.RepeatInterval,
            PreviousExecutionTime = alert.PreviousExecution.DateTime,
            NextExecutionTime = alert.NextExecution.DateTime,
        };
    }
}