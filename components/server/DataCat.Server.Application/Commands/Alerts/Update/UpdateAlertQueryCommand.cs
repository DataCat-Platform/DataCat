namespace DataCat.Server.Application.Commands.Alerts.Update;

public sealed record UpdateAlertQueryCommand : ICommand, IAuthorizedCommand
{
    public required string AlertId { get; init; }
    
    public required string? Description { get; init; }
    
    public required string Template { get; init; }
    
    public required string RawQuery { get; init; }

    public required string DataSourceId { get; init; }
}