namespace DataCat.Server.Application.Commands.DataSource.Update;

public sealed record UpdateDataSourceCommand : IRequest<Result>
{
    public required string DataSourceId { get; init; }

    public required string ConnectionString { get; init; }
}