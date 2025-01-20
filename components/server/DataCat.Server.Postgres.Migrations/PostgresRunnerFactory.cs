namespace DataCat.Server.Postgres.Migrations;

public class PostgresRunnerFactory(MigrationOptions options) : IMigrationRunnerFactory
{
    public IDataCatMigrationRunner CreateMigrationRunner()
    {
        return new PostgresMigrationRunner(options);        
    }
}