namespace DataCat.Server.Application.Commands.Dashboard.Add;

public sealed record AddDashboardCommand : IRequest<Result<Guid>>, IAuthorizedCommand
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public Guid? NamespaceId { get; set; } = null;
    
    public required string UserId { get; init; }
}