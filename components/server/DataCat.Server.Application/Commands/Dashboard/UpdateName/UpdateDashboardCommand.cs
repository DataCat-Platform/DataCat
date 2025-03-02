namespace DataCat.Server.Application.Commands.Dashboard.UpdateName;

public sealed record UpdateDashboardCommand : IRequest<Result>, IAuthorizedCommand
{
    public required string DashboardId { get; init; }

    public required string Name { get; init; }
    
    public string? Description { get; init; }
}