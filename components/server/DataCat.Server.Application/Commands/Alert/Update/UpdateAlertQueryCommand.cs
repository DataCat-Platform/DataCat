namespace DataCat.Server.Application.Commands.Alert.Update;

public sealed record UpdateAlertQueryCommand : IRequest<Result>, IAuthorizedCommand
{
    public required string AlertId { get; init; }
    
    public required string? Description { get; init; }
    
    public required string RawQuery { get; init; }

    public required string DataSourceId { get; init; }
    
    public required string NotificationChannelId { get; init; }
}