namespace DataCat.Server.Domain.Core.Errors;

public sealed class TimeRangeError(string code, string message) : BaseError(code, message)
{
    public static TimeRangeError DateIsNull(string name) => new("TimeRange.Date", $"DateField {name} cannot be null");
    
    public static readonly TimeRangeError FromGreaterThanTo = new("TimeRange.Date", "'from' cannot be greater than 'to'");
}