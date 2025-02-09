namespace DataCat.Server.Domain.Core;

public class QueryEntity
{
    private QueryEntity(DataSourceEntity dataSourceEntity, string rawQuery)
    {
        DataSourceEntity = dataSourceEntity;
        RawQuery = rawQuery;
    }

    public Guid Id { get; private set; }

    public DataSourceEntity DataSourceEntity { get; private set; }

    public string RawQuery { get; private set; }

    public static Result<QueryEntity> Create(DataSourceEntity? dataSource, string? rawQuery)
    {
        var validationList = new List<Result<QueryEntity>>();

        #region Validation

        if (dataSource is null)
        {
            validationList.Add(Result.Fail<QueryEntity>("DataSourceEntity cannot be null"));
        }

        if (string.IsNullOrWhiteSpace(rawQuery))
        {
            validationList.Add(Result.Fail<QueryEntity>("RawQuery cannot be null or empty"));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new QueryEntity(dataSource!, rawQuery!));
    }
}
