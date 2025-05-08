namespace DataCat.Server.Domain.Core;

public sealed class DataCatLayout : ValueObject
{
    public string Settings { get; }

    public DataCatLayout(string settings)
    {
        Settings = settings;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Settings;
    }
}
