namespace DataCat.Server.Application.Commands.DataSources.UpdateConnectionString;

public sealed record UpdateConnectionStringDataSourceCommand : ICommand, IAdminRequest
{
    public required string DataSourceName { get; init; }

    public required string ConnectionString { get; init; }
}