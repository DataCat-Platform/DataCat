namespace DataCat.Server.Domain.Core.ValueObjects;

public sealed class AlertSchedule : ValueObject
{
    public TimeSpan WaitTimeBeforeAlerting { get; }
    public TimeSpan RepeatInterval { get; }

    public AlertSchedule(TimeSpan waitTimeBeforeAlerting, TimeSpan repeatInterval)
    {
        WaitTimeBeforeAlerting = waitTimeBeforeAlerting;
        RepeatInterval = repeatInterval;
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return WaitTimeBeforeAlerting;
        yield return RepeatInterval;
    }
}