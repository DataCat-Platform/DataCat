namespace DataCat.Server.Infrastructure.Options;

public sealed record PluginStoreOptions
{
    public required string PluginPath { get; init; }
}