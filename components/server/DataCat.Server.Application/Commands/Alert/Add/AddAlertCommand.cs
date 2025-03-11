namespace DataCat.Server.Application.Commands.Alert.Add;

public sealed record AddAlertCommand : IRequest<Result<Guid>>, IAuthorizedCommand
{
    public required string? Description { get; init; }
    
    public required string RawQuery { get; init; }
    
    public required string DataSourceId { get; init; }
    
    public required string NotificationChannelId { get; init; }
    
    public required TimeSpan WaitTimeBeforeAlerting { get; init; }
    
    public required TimeSpan RepeatInterval { get; init; }
}