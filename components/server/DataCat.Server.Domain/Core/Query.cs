namespace DataCat.Server.Domain.Core;

public sealed record Query
{
    private Query(DataSource dataSource, string rawQuery)
    {
        DataSource = dataSource;
        RawQuery = rawQuery;
    }

    public DataSource DataSource { get; private set; }

    public string RawQuery { get; private set; }

    public static Result<Query> Create(DataSource? dataSource, string? rawQuery)
    {
        var validationList = new List<Result<Query>>();

        #region Validation

        if (dataSource is null)
        {
            validationList.Add(Result.Fail<Query>("DataSourceEntity cannot be null"));
        }

        if (string.IsNullOrWhiteSpace(rawQuery))
        {
            validationList.Add(Result.Fail<Query>("RawQuery cannot be null or empty"));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new Query(dataSource!, rawQuery!));
    }
}
