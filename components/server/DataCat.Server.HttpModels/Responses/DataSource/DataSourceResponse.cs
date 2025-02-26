namespace DataCat.Server.HttpModels.Responses.DataSource;

public class DataSourceResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string ConnectionString { get; init; }
}