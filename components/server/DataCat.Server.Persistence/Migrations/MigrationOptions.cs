namespace DataCat.Server.Persistence.Migrations;

/// <summary>
/// TODO: Add scheme and etc options
/// </summary>
public sealed record MigrationOptions
{
    public required string ConnectionString { get; set; }
}