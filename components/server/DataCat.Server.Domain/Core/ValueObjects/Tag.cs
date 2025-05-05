namespace DataCat.Server.Domain.Core.ValueObjects;

public sealed class Tag : ValueObject
{
    public string Value { get; }

    public Tag(string value)
    {
        Value = value;
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}