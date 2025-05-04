using System.Diagnostics.Metrics;

namespace DataCat.Secrets.Vault.Telemetry;

public sealed class VaultMetricsContainer
{
    // Authentication metrics
    private readonly Counter<long> _authAttempts;
    private readonly Counter<long> _authFailures;
    private readonly Histogram<long> _authDurations;
    
    // Secret access metrics
    private readonly Counter<long> _secretRequests;
    private readonly Counter<long> _secretCacheHits;
    private readonly Histogram<long> _secretAccessDurations;
    private readonly Counter<long> _secretAccessFailures;
    
    // Cache metrics
    private readonly Counter<long> _cacheCleanups;
    private readonly Counter<long> _cacheEvictions;
    private long _cacheSizeValue;
    private readonly ObservableGauge<long> _cacheSize;
    
    // API metrics
    private readonly Counter<long> _vaultApiCalls;
    private readonly Counter<long> _vaultApiFailures;
    private readonly Histogram<long> _vaultApiDurations;

    public VaultMetricsContainer(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(VaultMetricsConstants.MeterName);

        // Authentication metrics
        _authAttempts = meter.CreateCounter<long>(
            "datacat.vault.auth.attempts.total",
            description: "Total Vault authentication attempts");
            
        _authFailures = meter.CreateCounter<long>(
            "datacat.vault.auth.failures.total",
            description: "Total Vault authentication failures");
            
        _authDurations = meter.CreateHistogram<long>(
            "datacat.vault.auth.duration.ms",
            unit: "ms",
            description: "Vault authentication duration");

        // Secret access metrics
        _secretRequests = meter.CreateCounter<long>(
            "datacat.vault.secrets.requests.total",
            description: "Total secret requests");
            
        _secretCacheHits = meter.CreateCounter<long>(
            "datacat.vault.secrets.cache.hits.total",
            description: "Total secret cache hits");
            
        _secretAccessDurations = meter.CreateHistogram<long>(
            "datacat.vault.secrets.access.duration.ms",
            unit: "ms",
            description: "Secret access duration");
            
        _secretAccessFailures = meter.CreateCounter<long>(
            "datacat.vault.secrets.access.failures.total",
            description: "Total secret access failures");

        // Cache metrics
        _cacheCleanups = meter.CreateCounter<long>(
            "datacat.vault.cache.cleanups.total",
            description: "Total cache cleanup operations");
            
        _cacheEvictions = meter.CreateCounter<long>(
            "datacat.vault.cache.evictions.total",
            description: "Total cache evictions");
            
        _cacheSize = meter.CreateObservableGauge(
            "datacat.vault.cache.size",
            () => _cacheSizeValue,
            description: "Current number of items in cache"
        );

        // API metrics
        _vaultApiCalls = meter.CreateCounter<long>(
            "datacat.vault.api.calls.total",
            description: "Total Vault API calls");
            
        _vaultApiFailures = meter.CreateCounter<long>(
            "datacat.vault.api.failures.total",
            description: "Total Vault API call failures");
            
        _vaultApiDurations = meter.CreateHistogram<long>(
            "datacat.vault.api.duration.ms",
            unit: "ms",
            description: "Vault API call duration");
    }
    
    // Authentication methods
    public void AddAuthAttempt(string authType)
    {
        _authAttempts.Add(1, new TagList { { "type", authType } });
    }
    
    public void AddAuthFailure(string authType)
    {
        _authFailures.Add(1, new TagList { { "type", authType } });
    }
    
    public void RecordAuthDuration(string authType, long durationMs, bool isSuccess)
    {
        var tags = new TagList
        {
            { "type", authType },
            { "status", isSuccess ? "success" : "failed" }
        };
        _authDurations.Record(durationMs, tags);
    }
    
    // Secret access methods
    public void AddSecretRequest()
    {
        _secretRequests.Add(1);
    }
    
    public void AddSecretCacheHit()
    {
        _secretCacheHits.Add(1);
    }
    
    public void RecordSecretAccessDuration(long durationMs, bool isSuccess)
    {
        _secretAccessDurations.Record(durationMs, new TagList { { "status", isSuccess ? "success" : "failed" } });
    }
    
    public void AddSecretAccessFailure()
    {
        _secretAccessFailures.Add(1);
    }
    
    // Cache methods
    public void AddCacheCleanup()
    {
        _cacheCleanups.Add(1);
    }
    
    public void AddCacheEviction(int count = 1)
    {
        _cacheEvictions.Add(count);
    }
    
    public void SetCacheSize(long size)
    {
        Interlocked.Exchange(ref _cacheSizeValue, size);
    }
    
    // API methods
    public void AddVaultApiCall(string operation)
    {
        _vaultApiCalls.Add(1, new TagList { { "operation", operation } });
    }
    
    public void AddVaultApiFailure(string operation)
    {
        _vaultApiFailures.Add(1, new TagList { { "operation", operation } });
    }
    
    public void RecordVaultApiDuration(string operation, long durationMs, bool isSuccess)
    {
        var tags = new TagList
        {
            { "operation", operation },
            { "status", isSuccess ? "success" : "failed" }
        };
        _vaultApiDurations.Record(durationMs, tags);
    }
}