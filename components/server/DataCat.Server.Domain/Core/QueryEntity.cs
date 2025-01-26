namespace DataCat.Server.Domain.Core;

public class QueryEntity
{
    private QueryEntity(DataSource dataSource, string rawQuery)
    {
        DataSource = dataSource;
        RawQuery = rawQuery;
    }

    public Guid Id { get; private set; }

    public DataSource DataSource { get; private set; }

    public string RawQuery { get; private set; }

    public static Result<QueryEntity> Create(DataSource? dataSource, string? rawQuery)
    {
        var validationList = new List<Result<QueryEntity>>();

        #region Validation

        if (dataSource is null)
        {
            validationList.Add(Result.Fail<QueryEntity>("DataSource cannot be null"));
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
