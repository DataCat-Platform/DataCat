namespace DataCat.Server.Application.Metrics;

public interface IMetricClient
{
    DataSourceType DataSourceType { get; }
    
    /// <summary>
    /// Method to get random metrics
    /// </summary>
    /// <param name="count"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<IEnumerable<MetricModel>> GetRandomMetricsAsync(int count, CancellationToken token);
    
    /// <summary>
    /// Method to check if an alert has been triggered based on <paramref name="rawQuery"/>
    /// </summary>
    /// <param name="rawQuery">Query to check if alert was triggered or not</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<bool> CheckAlertTriggerAsync(string rawQuery, CancellationToken token);
    
    /// <summary>
    /// Method to get alerts that need to be checked (inactive or triggered)
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<IEnumerable<AlertModel>> GetAlertsToCheckAsync(CancellationToken token);
}