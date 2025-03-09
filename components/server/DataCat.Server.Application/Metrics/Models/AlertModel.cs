namespace DataCat.Server.Application.Metrics.Models;

public class AlertModel
{
    public required int AlertId { get; init; }
    public required string AlertRawQuery { get; init; }
    public required string AlertStatus { get; init; }
    public required DateTime AlertNextExecution { get; init; }
    public required long AlertWaitTimeBeforeAlertingInTicks { get; init; }
}