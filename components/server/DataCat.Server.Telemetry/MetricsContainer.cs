namespace DataCat.Server.Telemetry;

public class MetricsContainer : IMetricsContainer
{
    // Command/Query metrics
    private readonly Counter<long> _commandExecutions;
    private readonly Histogram<long> _commandDurations;
    private readonly Counter<long> _queryExecutions;
    private readonly Histogram<long> _queryDurations;
    
    // Alerting metrics
    private readonly Counter<long> _alertsTriggered;
    private readonly Counter<long> _notificationsSent;
    private readonly Histogram<long> _notificationDeliveryTimes;
    
    // Cache metrics
    private readonly Counter<long> _cacheHits;
    private readonly Histogram<long> _cacheOperationTimes;
    
    // System metrics
    private readonly Histogram<long> _backgroundJobDurations;
    private readonly Counter<long> _errors;
    private readonly Counter<long> _validationErrors;
    
    public MetricsContainer(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MetricsConstants.MeterName);

        // Commands/Queries
        _commandExecutions = meter.CreateCounter<long>(
            "datacat.app.commands.executed.total",
            description: "Total commands executed");
            
        _commandDurations = meter.CreateHistogram<long>(
            "datacat.app.commands.duration.ms",
            unit: "ms",
            description: "Command execution duration");
            
        _queryExecutions = meter.CreateCounter<long>(
            "datacat.app.queries.executed.total",
            description: "Total queries executed");
            
        _queryDurations = meter.CreateHistogram<long>(
            "datacat.app.queries.duration.ms",
            unit: "ms",
            description: "Query execution duration");

        // Alerting
        _alertsTriggered = meter.CreateCounter<long>(
            "datacat.alerts.triggered.total",
            description: "Total alerts triggered");
            
        _notificationsSent = meter.CreateCounter<long>(
            "datacat.notifications.sent.total",
            description: "Total notifications sent");
            
        _notificationDeliveryTimes = meter.CreateHistogram<long>(
            "datacat.notifications.delivery.time.ms",
            unit: "ms",
            description: "Notification delivery duration");

        // Cache
        _cacheHits = meter.CreateCounter<long>(
            "datacat.cache.operations.total",
            description: "Total cache operations");
            
        _cacheOperationTimes = meter.CreateHistogram<long>(
            "datacat.cache.operations.time.ms",
            unit: "ms",
            description: "Cache operation duration");

        // System
        _backgroundJobDurations = meter.CreateHistogram<long>(
            "datacat.background.jobs.duration.ms",
            unit: "ms",
            description: "Background job execution duration");
            
        _errors = meter.CreateCounter<long>(
            "datacat.errors.total",
            description: "Total errors occurred");
            
        _validationErrors = meter.CreateCounter<long>(
            "datacat.validation.errors.total",
            description: "Total validation errors");
    }
    
    public void AddCommandExecution(string commandType, long durationMs, bool isSuccess)
    {
        var tags = new TagList
        {
            { "type", commandType },
            { "status", isSuccess ? "success" : "failed" }
        };
        _commandExecutions.Add(1, tags);
        _commandDurations.Record(durationMs, tags);
    }

    public void AddQueryExecution(string queryType, long durationMs, bool isSuccess)
    {
        var tags = new TagList
        {
            { "type", queryType },
            { "status", isSuccess ? "success" : "failed" }
        };
        _queryExecutions.Add(1, tags);
        _queryDurations.Record(durationMs, tags);
    }

    public void AddAlertTriggered()
    {
        _alertsTriggered.Add(1);
    }

    public void AddNotificationSent(string channelType, bool isSuccess)
    {
        var tags = new TagList
        {
            { "channel", channelType },
            { "status", isSuccess ? "success" : "failed" }
        };
        _notificationsSent.Add(1, tags);
    }

    public void AddNotificationDeliveryTime(long millis, string channelType)
    {
        _notificationDeliveryTimes.Record(millis, new TagList { { "channel", channelType } });
    }

    public void AddCacheHit(bool isHit, string cacheRegion)
    {
        var tags = new TagList
        {
            { "result", isHit ? "hit" : "miss" },
            { "region", cacheRegion }
        };
        _cacheHits.Add(1, tags);
    }

    public void AddCacheOperationTime(string operationType, long durationMs)
    {
        _cacheOperationTimes.Record(durationMs, new TagList { { "operation", operationType } });
    }

    public void AddBackgroundJobExecution(string jobType, long durationMs)
    {
        _backgroundJobDurations.Record(durationMs, new TagList { { "job_type", jobType } });
    }

    public void AddError(string errorType, string source)
    {
        var tags = new TagList
        {
            { "error_type", errorType },
            { "source", source }
        };
        _errors.Add(1, tags);
    }

    public void AddValidationError()
    {
        _validationErrors.Add(1);
    }
}