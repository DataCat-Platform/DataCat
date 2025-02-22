namespace DataCat.Server.Application.Commands.Alert.UpdateAlertQuery;

public sealed record UpdateAlertQueryCommand : IRequest<Result>
{
    public required string AlertId { get; init; }
    
    public required string? Description { get; init; }
    
    public required string RawQuery { get; init; }

    public required string DataSourceId { get; init; }
    
    public required string NotificationChannelId { get; init; }
}