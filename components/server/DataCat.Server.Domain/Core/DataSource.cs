namespace DataCat.Server.Domain.Core;

public class DataSource
{
    private DataSource(Guid id, string name, DataSourceType dataSourceType, string connectionSettings, DataSourcePurpose purpose)
    {
        Id = id;
        Name = name;
        DataSourceType = dataSourceType;
        ConnectionSettings = connectionSettings;
        Purpose = purpose;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public DataSourceType DataSourceType { get; private set; }

    public string ConnectionSettings { get; private set; }
    public DataSourcePurpose Purpose { get; }

    public void ChangeConnectionString(string connectionString)
    {
        ConnectionSettings = connectionString;
    }

    public static Result<DataSource> Create(Guid id, string name, DataSourceType? dataSourceType, string? connectionString, DataSourcePurpose? purpose)
    {
        var validationList = new List<Result<DataSource>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<DataSource>(BaseError.FieldIsNull(nameof(name))));
        }

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            validationList.Add(Result.Fail<DataSource>(BaseError.FieldIsNull(nameof(connectionString))));
        }

        if (dataSourceType is null)
        {
            validationList.Add(Result.Fail<DataSource>(BaseError.FieldIsNull(nameof(dataSourceType))));
        }
        
        if (purpose is null)
        {
            validationList.Add(Result.Fail<DataSource>(BaseError.FieldIsNull(nameof(purpose))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new DataSource(id, name, dataSourceType!, connectionString!, purpose!));
    }
}
