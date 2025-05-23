namespace DataCat.Server.Domain.Core.Services;

public interface IAlertMonitorService
{
    /// <summary>
    /// Retrieves a list of alerts that are due for checking.
    /// </summary>
    Task<IEnumerable<Alert>> GetAlertsToCheckAsync(int limit = 5, CancellationToken token = default);

    /// <summary>
    /// Retrieves a list of alerts that are currently in the Triggered state.
    /// </summary>
    Task<IEnumerable<Alert>> GetTriggeredAlertsAsync(int limit = 5, CancellationToken token = default);
}