namespace DataCat.Server.Application.Commands.DataSources.Add;

public sealed record AddDataSourceCommand : ICommand<Guid>, IAdminRequest
{
    public required string Name { get; init; }

    public required string DataSourceType { get; init; }

    public required string ConnectionString { get; init; }
    
    public required DataSourceKind Purpose {get; init; }
}