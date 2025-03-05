namespace DataCat.Server.Domain.Core.Services;

public interface IAlertMonitor
{
    Task<IEnumerable<AlertEntity>> GetActiveAlertsAsync(int top = 5, CancellationToken token = default);
    
    Task<AlertStatus> GetAlertStatusAsync(Guid alertId, CancellationToken token = default);
}