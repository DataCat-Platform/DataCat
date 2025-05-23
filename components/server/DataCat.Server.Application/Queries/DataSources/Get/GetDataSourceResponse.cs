namespace DataCat.Server.Application.Queries.DataSources.Get;

public sealed record GetDataSourceResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string ConnectionString { get; init; }
}

public static class GetDataSourceResponseExtensions
{
    public static GetDataSourceResponse ToResponse(this DataSource dataSource)
    {
        return new GetDataSourceResponse
        {
            Id = dataSource.Id,
            Name = dataSource.Name,
            ConnectionString = dataSource.ConnectionSettings,
            Type = dataSource.DataSourceType.Name
        };
    }
}