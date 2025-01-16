namespace DataCat.Server.Domain.Models;

public class DataSource
{
    private DataSource(Guid id, string name, DataSourceType dataSourceType, string connectionString)
    {
        Id = id;
        Name = name;
        DataSourceType = dataSourceType;
        ConnectionString = connectionString;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public DataSourceType DataSourceType { get; private set; }

    public string ConnectionString { get; private set; }

    public static Result<DataSource> Create(Guid id, string name, DataSourceType? dataSourceType, string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Fail<DataSource>("Name cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return Result.Fail<DataSource>("ConnectionString cannot be null or empty");
        }

        if (dataSourceType is null)
        {
            return Result.Fail<DataSource>("DataSourceType cannot be null");
        }

        return Result.Success(new DataSource(id, name, dataSourceType, connectionString));
    }
}
