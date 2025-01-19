namespace DataCat.Server.Postgres.Migrations;

public class PostgresMigrationRunner : IMigrationRunner
{
    public Task ApplyMigrationsAsync(MigrationOptions options, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task RollbackLastMigrationAsync(MigrationOptions options, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetCurrentMigrationVersionAsync(MigrationOptions options, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task ApplyMigrationAsync(MigrationOptions options, string migrationVersion, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}