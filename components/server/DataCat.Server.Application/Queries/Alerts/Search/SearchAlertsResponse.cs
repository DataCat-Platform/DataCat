namespace DataCat.Server.Application.Queries.Alerts.Search;

public sealed record SearchAlertsResponse
{
    public required Guid Id { get; init; }
    public required string? Description { get; init; }
    public required string Template { get; init; }
    public required string RawQuery { get; init; }
    public required string Status { get; init; }
    public required DataSourceResponse DataSource { get; init; }
    public required NotificationChannelGroupResponse NotificationChannelGroup { get; init; }
    public required DateTime PreviousExecutionTime { get; init; }
    public required DateTime NextExecutionTime { get; init; }
    public required TimeSpan WaitTimeBeforeAlerting { get; init; }
    public required TimeSpan RepeatInterval { get; init; }
    public required List<string> Tags { get; init; }
    public required Guid NamespaceId { get; init; }

    
    public static SearchAlertsResponse ToResponse(Alert alert)
    {
        return new SearchAlertsResponse
        {
            Id = alert.Id,
            Description = alert.Description,
            Template = alert.Template,
            RawQuery = alert.ConditionQuery.RawQuery,
            Status = alert.Status.Name,
            DataSource = alert.ConditionQuery.DataSource.ToResponse(),
            NotificationChannelGroup = alert.NotificationChannelGroup.ToResponse(),
            WaitTimeBeforeAlerting = alert.Schedule.WaitTimeBeforeAlerting,
            RepeatInterval = alert.Schedule.RepeatInterval,
            PreviousExecutionTime = alert.PreviousExecution.DateTime,
            NextExecutionTime = alert.NextExecution.DateTime,
            Tags = alert.Tags.Select(x => x.Value).OrderBy(x => x).ToList(),
            NamespaceId = alert.NamespaceId,
        };
    }
}
