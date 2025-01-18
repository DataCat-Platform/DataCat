namespace DataCat.Server.Domain.Core;

public class Query
{
    private Query(Guid id, DataSource dataSource, string rawQuery, TimeRange timeRange)
    {
        Id = id;
        DataSource = dataSource;
        RawQuery = rawQuery;
        TimeRange = timeRange;
    }

    public Guid Id { get; private set; }

    public DataSource DataSource { get; private set; }

    public string RawQuery { get; private set; }

    public TimeRange TimeRange { get; private set; }

    public static Result<Query> Create(Guid id, DataSource? dataSource, string? rawQuery, TimeRange? timeRange)
    {
        var validationList = new List<Result<Query>>();

        #region Validation

        if (dataSource is null)
        {
            validationList.Add(Result.Fail<Query>("DataSource cannot be null"));
        }

        if (string.IsNullOrWhiteSpace(rawQuery))
        {
            validationList.Add(Result.Fail<Query>("RawQuery cannot be null or empty"));
        }

        if (timeRange is null)
        {
            validationList.Add(Result.Fail<Query>("TimeRange cannot be null"));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new Query(id, dataSource!, rawQuery!, timeRange!));
    }
}
