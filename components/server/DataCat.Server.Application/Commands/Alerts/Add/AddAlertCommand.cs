namespace DataCat.Server.Application.Commands.Alerts.Add;

public sealed record AddAlertCommand : ICommand<Guid>, IAuthorizedCommand
{
    public required string? Description { get; init; }
    
    public required string Template { get; init; }
    
    public required string RawQuery { get; init; }
    
    public required string DataSourceId { get; init; }
    
    public required string NotificationChannelGroupName { get; init; }
    
    public required TimeSpan WaitTimeBeforeAlerting { get; init; }
    
    public required TimeSpan RepeatInterval { get; init; }
    
    public required List<string> Tags { get; init; }
}