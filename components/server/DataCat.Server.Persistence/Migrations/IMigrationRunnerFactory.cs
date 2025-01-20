namespace DataCat.Server.Persistence.Migrations;

/// <summary>
/// Factory interface for creating instances of <see cref="IDataCatMigrationRunner"/>.
/// </summary>
public interface IMigrationRunnerFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IDataCatMigrationRunner"/> for the specified data source.
    /// </summary>
    /// <returns>An instance of <see cref="IDataCatMigrationRunner"/> configured for the specified data source.</returns>
    IDataCatMigrationRunner CreateMigrationRunner();
}