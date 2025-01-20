namespace DataCat.Server.Persistence.Migrations;

/// <summary>
/// Represents configuration options for database migration operations.
/// </summary>
public sealed record MigrationOptions
{
    /// <summary>
    /// The connection string used to connect to the target database.
    /// </summary>
    public required string ConnectionString { get; init; }

    /// <summary>
    /// The database schema in which the migrations will be applied.
    /// If not specified, the default schema will be used.
    /// </summary>
    public string Scheme { get; init; } = string.Empty;
    
    /// <summary>
    /// The timeout duration for migration operations, in seconds.
    /// This value determines how long to wait before timing out during migration execution.
    /// Defaults to 30 seconds.
    /// </summary>
    public int TimeoutInSeconds { get; init; } = 30;
}