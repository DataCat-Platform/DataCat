namespace DataCat.Server.Persistence.Migrations;

public interface IMigrationRunnerFactory
{
    IMigrationRunner CreateMigrationRunner(DataSource dataSource);
}