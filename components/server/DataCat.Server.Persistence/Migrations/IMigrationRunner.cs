namespace DataCat.Server.Persistence.Migrations;

public interface IMigrationRunner
{
    /// <summary>
    /// Applies all pending migrations to the target database.
    /// </summary>
    /// <param name="options">Options for database connecting</param>
    /// <param name="token"></param>
    Task ApplyMigrationsAsync(MigrationOptions options, CancellationToken token = default);

    /// <summary>
    /// Rolls back the last applied migration.
    /// </summary>
    /// <param name="options">Options for database connecting</param>
    /// <param name="token"></param>
    Task RollbackLastMigrationAsync(MigrationOptions options, CancellationToken token = default);

    /// <summary>
    /// Checks the current migration status of the database.
    /// </summary>
    /// <param name="options">Options for database connecting</param>
    /// <param name="token"></param>
    /// <returns>The current migration version.</returns>
    Task<string?> GetCurrentMigrationVersionAsync(MigrationOptions options, CancellationToken token = default);

    /// <summary>
    /// Applies a specific migration by version.
    /// </summary>
    /// <param name="options">Options for database connecting</param>
    /// <param name="migrationVersion">The migration version to apply.</param>
    /// <param name="token"></param>
    Task ApplyMigrationAsync(MigrationOptions options, string migrationVersion, CancellationToken token = default);
}