namespace DataCat.Server.Application.Commands.DataSources.UpdateConnectionString;

public sealed record UpdateConnectionStringDataSourceCommand : IRequest<Result>, IAdminRequest
{
    public required string DataSourceId { get; init; }

    public required string ConnectionString { get; init; }
}