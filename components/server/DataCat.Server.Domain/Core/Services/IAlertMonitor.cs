namespace DataCat.Server.Domain.Core.Services;

public interface IAlertMonitor
{
    Task ConsumeAlertsAsync(CancellationToken token = default);
    
    Task<IEnumerable<AlertEntity>> GetActiveAlertsAsync(CancellationToken token = default);
    
    Task<AlertStatus> GetAlertStatusAsync(Guid alertId, CancellationToken token = default);
}