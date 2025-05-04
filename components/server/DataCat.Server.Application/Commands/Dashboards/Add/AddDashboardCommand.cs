namespace DataCat.Server.Application.Commands.Dashboards.Add;

public sealed record AddDashboardCommand : ICommand<Guid>, IAuthorizedCommand
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public Guid? NamespaceId { get; set; } = null;
    
    public required string UserId { get; init; }
}