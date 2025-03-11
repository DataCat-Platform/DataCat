namespace DataCat.Server.Domain.Common;

public readonly struct DateTimeUtc(DateTime dateTime) : IEquatable<DateTimeUtc>
{
    public DateTime DateTime { get; } = dateTime.ToUniversalTime();
    
    public static bool operator >(DateTimeUtc left, DateTimeUtc right) => left.DateTime > right.DateTime;
    public static bool operator <(DateTimeUtc left, DateTimeUtc right) => left.DateTime < right.DateTime;
    public static bool operator >=(DateTimeUtc left, DateTimeUtc right) => left.DateTime >= right.DateTime;
    public static bool operator <=(DateTimeUtc left, DateTimeUtc right) => left.DateTime <= right.DateTime;
    public static bool operator ==(DateTimeUtc left, DateTimeUtc right) => left.DateTime == right.DateTime;
    public static bool operator !=(DateTimeUtc left, DateTimeUtc right) => left.DateTime != right.DateTime;
    
    public static implicit operator DateTimeUtc(DateTime dateTime) => new(dateTime);

    public static DateTimeUtc Init()
    {
        return DateTime.MinValue;
    } 

    public override bool Equals(object? obj)
    {
        return obj is DateTimeUtc other && Equals(other);
    }

    public override int GetHashCode()
    {
        return DateTime.GetHashCode();
    }

    public bool Equals(DateTimeUtc other)
    {
        return DateTime.Equals(other.DateTime);
    }
}