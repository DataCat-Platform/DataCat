namespace DataCat.Server.Domain.Models;

public class TimeRange
{
    private TimeRange(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }

    public DateTime From { get; private set; }
    
    public DateTime To { get; private set; }

    public static Result<TimeRange> Create(DateTime? from, DateTime? to)
    {
        if (!from.HasValue)
        {
            return Result.Fail<TimeRange>("From date cannot be null");
        }

        if (!to.HasValue)
        {
            return Result.Fail<TimeRange>("To date cannot be null");
        }

        if (from.Value > to.Value)
        {
            return Result.Fail<TimeRange>("From date cannot be later than To date");
        }

        return Result.Success(new TimeRange(from.Value, to.Value));
    }
}