namespace DataCat.Storage.Postgres.Services;

public class PostgresAlertMonitor : IAlertMonitor
{
    public Task ConsumeAlertsAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AlertEntity>> GetActiveAlertsAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<AlertStatus> GetAlertStatusAsync(Guid alertId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}