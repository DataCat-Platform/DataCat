namespace DataCat.Storage.Postgres.Runners;

/// <summary>
/// A migration runner for PostgreSQL using FluentMigrator.
/// </summary>
public sealed record PostgresMigrationRunner : IDataCatMigrationRunner
{
    private readonly IServiceProvider _serviceProvider;

    public PostgresMigrationRunner(DatabaseOptions options)
        : this(CreateServiceProvider(options)) { }

    private PostgresMigrationRunner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Applies all pending migrations to the target database.
    /// </summary>
    public async Task ApplyMigrationsAsync(CancellationToken token = default)
    {
        await Task.CompletedTask;
        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }

    /// <summary>
    /// Rolls back the last applied migration.
    /// </summary>
    public async Task RollbackLastMigrationAsync(int steps, CancellationToken token = default)
    {
        await Task.CompletedTask;
        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.Rollback(steps);
    }

    /// <summary>
    /// Gets the current migration version from the database.
    /// </summary>
    public async Task<string?> GetCurrentMigrationVersionAsync(CancellationToken token = default)
    {
        await Task.CompletedTask;
        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        var migrationVersion = runner.MigrationLoader.LoadMigrations().LastOrDefault().Value.Version.ToString();

        return migrationVersion;
    }

    /// <summary>
    /// Applies a specific migration by version.
    /// </summary>
    public async Task ApplyMigrationAsync(string migrationVersion, CancellationToken token = default)
    {
        if (!long.TryParse(migrationVersion, out var version))
            throw new ArgumentException("Invalid migration version format.", nameof(migrationVersion));

        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp(version);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Creates the FluentMigrator service provider.
    /// </summary>
    private static ServiceProvider CreateServiceProvider(DatabaseOptions options)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(
                rb =>
                {
                    rb.AddPostgres()
                        .WithGlobalConnectionString(options.ConnectionString)
                        .WithGlobalCommandTimeout(TimeSpan.FromSeconds(options.TimeoutInSeconds))
                        .ScanIn(typeof(PostgresMigrationRunner).Assembly)
                            .For.Migrations();
                })
            .BuildServiceProvider();
    }
}