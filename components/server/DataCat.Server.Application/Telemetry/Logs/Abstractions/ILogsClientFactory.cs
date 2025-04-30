namespace DataCat.Server.Application.Telemetry.Logs.Abstractions;

/// <summary>
/// Factory for creating instances of <see cref="ILogsClient"/>.
/// </summary>
public interface ILogsClientFactory
{
    /// <summary>
    /// Checks if the factory can create a client for the specified data source.
    /// </summary>
    /// <param name="dataSource">The data source to check.</param>
    /// <returns>True if the factory can create a client; otherwise, false.</returns>
    bool CanCreate(DataSource dataSource);
    
    /// <summary>
    /// Creates a log client configured for the specified data source.
    /// </summary>
    /// <param name="dataSource">The data source configuration.</param>
    /// <returns>A new instance of <see cref="ILogsClient"/>.</returns>
    ILogsClient CreateClient(DataSource dataSource);
}