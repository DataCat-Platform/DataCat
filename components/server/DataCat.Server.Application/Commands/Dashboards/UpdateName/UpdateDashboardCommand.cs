namespace DataCat.Server.Application.Commands.Dashboards.UpdateName;

public sealed record UpdateDashboardCommand : ICommand, IAuthorizedCommand
{
    public required string DashboardId { get; init; }

    public required string Name { get; init; }
    
    public string? Description { get; init; }
}