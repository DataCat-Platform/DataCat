namespace DataCat.Server.Domain.Core;

public class DataSourceEntity
{
    private DataSourceEntity(Guid id, string name, DataSourceType dataSourceType, string connectionString)
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

    public void ChangeConnectionString(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public static Result<DataSourceEntity> Create(Guid id, string name, DataSourceType? dataSourceType, string? connectionString)
    {
        var validationList = new List<Result<DataSourceEntity>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<DataSourceEntity>(BaseError.FieldIsNull(nameof(name))));
        }

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            validationList.Add(Result.Fail<DataSourceEntity>(BaseError.FieldIsNull(nameof(connectionString))));
        }

        if (dataSourceType is null)
        {
            validationList.Add(Result.Fail<DataSourceEntity>(BaseError.FieldIsNull(nameof(dataSourceType))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new DataSourceEntity(id, name, dataSourceType!, connectionString!));
    }
}
