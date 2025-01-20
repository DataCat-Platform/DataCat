namespace DataCat.Server.Persistence.Migrations;

/// <summary>
/// Defines a contract for managing database migrations.
/// Provides methods to apply, rollback, and check the status of migrations
/// for a target database.
/// </summary>
public interface IDataCatMigrationRunner
{
    /// <summary>
    /// Applies all pending migrations to the target database.
    /// </summary>
    /// <param name="token"></param>
    Task ApplyMigrationsAsync(CancellationToken token = default);

    /// <summary>
    /// Rolls back the last applied migration.
    /// </summary>
    /// <param name="steps">
    /// The number of migration steps to roll back. For example, passing 1 will roll back the last migration, 
    /// while passing 2 will roll back the last two migrations.
    /// </param>
    /// <param name="token"></param>
    Task RollbackLastMigrationAsync(int steps, CancellationToken token = default);

    /// <summary>
    /// Checks the current migration status of the database.
    /// </summary>
    /// <param name="token"></param>
    /// <returns>The current migration version.</returns>
    Task<string?> GetCurrentMigrationVersionAsync(CancellationToken token = default);

    /// <summary>
    /// Applies a specific migration by version.
    /// </summary>
    /// <param name="migrationVersion">The migration version to apply.</param>
    /// <param name="token"></param>
    Task ApplyMigrationAsync(string migrationVersion, CancellationToken token = default);
}