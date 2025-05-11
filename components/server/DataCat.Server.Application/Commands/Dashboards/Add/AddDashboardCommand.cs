namespace DataCat.Server.Application.Commands.Dashboards.Add;

public sealed record AddDashboardCommand : ICommand<Guid>, IAuthorizedCommand
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public required Guid? NamespaceId { get; init; }
    
    public required List<string> Tags { get; init; } = [];
}