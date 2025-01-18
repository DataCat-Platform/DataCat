namespace DataCat.Server.Domain.Core.Errors;

public class BaseError(string code, string message)
{
    public static readonly BaseError None = new(String.Empty, String.Empty);
    public static readonly BaseError ConditionNotMet = new("Error.ConditionNotMet", "The specified condition was not met.");
    public static readonly BaseError NullValue = new("Error.NullValue", "The specified result value is null.");

    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; set; } = message;
    
    /// <summary>
    /// Error code
    /// </summary>
    public string Code { get; set; } = code;
    
    public static implicit operator string(BaseError baseError) => baseError.Message;
    
    public static BaseError FieldIsNull(string fieldName) =>
        new BaseError("Error.NullValue", $"{fieldName} cannot be null");

    public bool Equals(BaseError? other) =>
        other is not null && other.Message.Equals(Message) && other.Code.Equals(Code);

    public static bool operator ==(BaseError? left, BaseError? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;

        return left.Equals(right);
    }

    public static bool operator !=(BaseError? left, BaseError? right)
    {
        return !(left == right);
    }
    
    public static BaseError operator +(BaseError? left, BaseError? right)
    {
        return new BaseError("Error.Combined", $"{left?.Message ?? string.Empty}{right?.Message ?? string.Empty}");
    }

    public override bool Equals(object? obj) =>
        obj is BaseError failure && Equals(failure);

    public override int GetHashCode()
    {
        return HashCode.Combine(Message, Code);
    }
}