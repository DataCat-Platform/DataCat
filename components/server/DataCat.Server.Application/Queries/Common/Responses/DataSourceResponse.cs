namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record DataSourceResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string ConnectionString { get; init; }
}

public static class DataSourceResponseExtensions
{
    public static DataSourceResponse ToResponse(this DataSource dataSource)
    {
        return new DataSourceResponse
        {
            Id = dataSource.Id,
            Name = dataSource.Name,
            ConnectionString = dataSource.ConnectionString,
            Type = dataSource.DataSourceType.Name
        };
    }
}