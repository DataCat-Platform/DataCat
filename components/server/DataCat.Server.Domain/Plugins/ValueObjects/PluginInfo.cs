namespace DataCat.Server.Domain.Plugins.ValueObjects;

public class PluginInfo : ValueObject
{
    public required string FilePath { get; init; }

    public required string FileName { get; init; }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return FilePath;
        yield return FileName;
    }
}