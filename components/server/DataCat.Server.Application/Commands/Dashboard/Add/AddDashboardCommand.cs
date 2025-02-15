namespace DataCat.Server.Application.Commands.Dashboard.Add;

public sealed record AddDashboardCommand : IRequest<Result<Guid>>
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public required string UserId { get; init; }
}