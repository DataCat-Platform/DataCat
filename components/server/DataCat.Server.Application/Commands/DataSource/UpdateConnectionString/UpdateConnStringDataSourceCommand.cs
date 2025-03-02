namespace DataCat.Server.Application.Commands.DataSource.UpdateConnectionString;

public sealed record UpdateConnStringDataSourceCommand : IRequest<Result>, IAdminRequest
{
    public required string DataSourceId { get; init; }

    public required string ConnectionString { get; init; }
}