namespace DataCat.Server.Application.Commands.DataSources.UpdateConnectionString;

public sealed record UpdateConnectionStringDataSourceCommand : ICommand, IAdminRequest
{
    public required Guid Id { get; init; }

    public required string ConnectionString { get; init; }
}