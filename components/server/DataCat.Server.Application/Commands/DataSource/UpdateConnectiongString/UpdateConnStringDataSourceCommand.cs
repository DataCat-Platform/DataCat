namespace DataCat.Server.Application.Commands.DataSource.UpdateConnectiongString;

public sealed record UpdateConnStringDataSourceCommand : IRequest<Result>
{
    public required string DataSourceId { get; init; }

    public required string ConnectionString { get; init; }
}