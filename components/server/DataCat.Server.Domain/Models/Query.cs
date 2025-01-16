namespace DataCat.Server.Domain.Models;

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
        if (dataSource is null)
        {
            return Result.Fail<Query>("DataSource cannot be null");
        }

        if (string.IsNullOrWhiteSpace(rawQuery))
        {
            return Result.Fail<Query>("RawQuery cannot be null or empty");
        }

        if (timeRange is null)
        {
            return Result.Fail<Query>("TimeRange cannot be null");
        }

        return Result.Success(new Query(id, dataSource, rawQuery, timeRange));
    }
}
