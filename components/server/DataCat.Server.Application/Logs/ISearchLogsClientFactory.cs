namespace DataCat.Server.Application.Logs;

/// <summary>
/// Factory for creating instances of <see cref="ISearchLogsClient"/>.
/// </summary>
public interface ISearchLogsClientFactory
{
    /// <summary>
    /// Creates a log search client with default index configuration.
    /// </summary>
    /// <returns>A new instance of <see cref="ISearchLogsClient"/>.</returns>
    ISearchLogsClient CreateClient();
    
    /// <summary>
    /// Creates a log search client configured for the specified index pattern.
    /// </summary>
    /// <param name="indexPattern">The index pattern the client should operate on.</param>
    /// <returns>A new instance of <see cref="ISearchLogsClient"/> configured for the specified index.</returns>
    ISearchLogsClient CreateClient(string indexPattern);
}