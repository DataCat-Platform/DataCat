namespace DataCat.Server.Application.Queries.DataSources.Search;

public sealed record SearchDataSourcesResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string ConnectionString { get; init; }
    
    public static SearchDataSourcesResponse ToResponse(DataSource dataSource)
    {
        return new SearchDataSourcesResponse
        {
            Id = dataSource.Id,
            Name = dataSource.Name,
            ConnectionString = dataSource.ConnectionSettings,
            Type = dataSource.DataSourceType.Name
        };
    }
}