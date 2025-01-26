namespace DataCat.Server.Postgres.Runners;

public class PostgresRunnerFactory(DatabaseOptions options) : IMigrationRunnerFactory
{
    public IDataCatMigrationRunner CreateMigrationRunner()
    {
        return new PostgresMigrationRunner(options);        
    }
}