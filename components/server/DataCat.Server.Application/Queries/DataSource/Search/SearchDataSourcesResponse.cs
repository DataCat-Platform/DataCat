namespace DataCat.Server.Application.Queries.DataSource.Search;

public sealed record SearchDataSourcesResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string ConnectionString { get; init; }
    
    public static SearchDataSourcesResponse ToResponse(DataSourceEntity dataSource)
    {
        return new SearchDataSourcesResponse
        {
            Id = dataSource.Id,
            Name = dataSource.Name,
            ConnectionString = dataSource.ConnectionString,
            Type = dataSource.DataSourceType.Name
        };
    }
}