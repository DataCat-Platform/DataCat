namespace DataCat.Storage.Postgres.Services;

public class PostgresAlertMonitor : IAlertMonitor
{
    public Task<IEnumerable<AlertEntity>> GetActiveAlertsAsync(int top = 5, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<AlertStatus> GetAlertStatusAsync(Guid alertId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}