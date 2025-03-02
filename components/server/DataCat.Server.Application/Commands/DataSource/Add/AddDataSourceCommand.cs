namespace DataCat.Server.Application.Commands.DataSource.Add;

public sealed record AddDataSourceCommand : IRequest<Result<Guid>>, IAdminRequest
{
    public required string Name { get; init; }

    public required int Type { get; init; }

    public required string ConnectionString { get; init; }
}