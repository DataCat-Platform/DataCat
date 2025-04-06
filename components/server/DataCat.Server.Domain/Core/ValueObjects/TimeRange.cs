namespace DataCat.Server.Domain.Core.ValueObjects;

public sealed class TimeRange : ValueObject
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
        var validationList = new List<Result<TimeRange>>();

        #region Validation

        if (!from.HasValue)
        {
            validationList.Add(Result.Fail<TimeRange>(TimeRangeError.DateIsNull(nameof(from))));
        }

        if (!to.HasValue)
        {
            validationList.Add(Result.Fail<TimeRange>(TimeRangeError.DateIsNull(nameof(to))));
        }

        if (from.HasValue && to.HasValue && from.Value > to.Value)
        {
            validationList.Add(Result.Fail<TimeRange>(TimeRangeError.FromGreaterThanTo));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new TimeRange(from!.Value, to!.Value));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return From;
        yield return To;
    }
}