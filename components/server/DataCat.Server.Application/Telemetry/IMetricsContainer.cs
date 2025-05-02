namespace DataCat.Server.Application.Telemetry;

public interface IMetricsContainer
{
    // Command/Query metrics
    void AddCommandExecution(string commandType, long durationMs, bool isSuccess);
    void AddQueryExecution(string queryType, long durationMs, bool isSuccess);
    
    // Alerting metrics
    void AddAlertTriggered(string alertType, string severity);
    void AddNotificationSent(string channelType, bool isSuccess);
    void AddNotificationDeliveryTime(long millis, string channelType);
    
    // Cache metrics
    void AddCacheHit(bool isHit, string cacheRegion);
    void AddCacheOperationTime(string operationType, long durationMs);
    
    // System metrics
    void AddBackgroundJobExecution(string jobType, long durationMs);
    
    // Error metrics
    void AddError(string errorType, string source);
    void AddValidationError(string validatorType);
}