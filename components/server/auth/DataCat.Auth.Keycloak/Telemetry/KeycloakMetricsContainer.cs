namespace DataCat.Auth.Keycloak.Telemetry;

public sealed class KeycloakMetricsContainer
{
    // Authentication metrics
    private readonly Counter<long> _authenticationAttempts;
    private readonly Counter<long> _authenticationFailures;
    private readonly Histogram<long> _authenticationDurations;
    
    // Token metrics
    private readonly Counter<long> _tokenRequests;
    private readonly Counter<long> _tokenCacheHits;
    private readonly Histogram<long> _tokenRequestDurations;
    
    // User synchronization metrics
    private readonly Counter<long> _userSyncOperations;
    private readonly Counter<long> _usersSynced;
    private readonly Histogram<long> _userSyncDurations;
    private readonly Counter<long> _userSyncFailures;
    
    // Role synchronization metrics
    private readonly Counter<long> _roleSyncOperations;
    private readonly Counter<long> _rolesSynced;
    private readonly Histogram<long> _roleSyncDurations;
    private readonly Counter<long> _roleSyncFailures;
    
    // HTTP request metrics
    private readonly Counter<long> _keycloakApiCalls;
    private readonly Counter<long> _keycloakApiFailures;
    private readonly Histogram<long> _keycloakApiDurations;

    public KeycloakMetricsContainer(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(KeycloakMetricsConstants.MeterName);

        // Authentication metrics
        _authenticationAttempts = meter.CreateCounter<long>(
            "datacat.keycloak.auth.attempts.total",
            description: "Total authentication attempts");
            
        _authenticationFailures = meter.CreateCounter<long>(
            "datacat.keycloak.auth.failures.total",
            description: "Total authentication failures");
            
        _authenticationDurations = meter.CreateHistogram<long>(
            "datacat.keycloak.auth.duration.ms",
            unit: "ms",
            description: "Authentication duration");

        // Token metrics
        _tokenRequests = meter.CreateCounter<long>(
            "datacat.keycloak.tokens.requests.total",
            description: "Total token requests");
            
        _tokenCacheHits = meter.CreateCounter<long>(
            "datacat.keycloak.tokens.cache.hits.total",
            description: "Total token cache hits");
            
        _tokenRequestDurations = meter.CreateHistogram<long>(
            "datacat.keycloak.tokens.request.duration.ms",
            unit: "ms",
            description: "Token request duration");

        // User synchronization metrics
        _userSyncOperations = meter.CreateCounter<long>(
            "datacat.keycloak.users.sync.operations.total",
            description: "Total user synchronization operations");
            
        _usersSynced = meter.CreateCounter<long>(
            "datacat.keycloak.users.synced.total",
            description: "Total users synced");
            
        _userSyncDurations = meter.CreateHistogram<long>(
            "datacat.keycloak.users.sync.duration.ms",
            unit: "ms",
            description: "User synchronization duration");
            
        _userSyncFailures = meter.CreateCounter<long>(
            "datacat.keycloak.users.sync.failures.total",
            description: "Total user synchronization failures");

        // Role synchronization metrics
        _roleSyncOperations = meter.CreateCounter<long>(
            "datacat.keycloak.roles.sync.operations.total",
            description: "Total role synchronization operations");
            
        _rolesSynced = meter.CreateCounter<long>(
            "datacat.keycloak.roles.synced.total",
            description: "Total roles synced");
            
        _roleSyncDurations = meter.CreateHistogram<long>(
            "datacat.keycloak.roles.sync.duration.ms",
            unit: "ms",
            description: "Role synchronization duration");
            
        _roleSyncFailures = meter.CreateCounter<long>(
            "datacat.keycloak.roles.sync.failures.total",
            description: "Total role synchronization failures");

        // HTTP request metrics
        _keycloakApiCalls = meter.CreateCounter<long>(
            "datacat.keycloak.api.calls.total",
            description: "Total Keycloak API calls");
            
        _keycloakApiFailures = meter.CreateCounter<long>(
            "datacat.keycloak.api.failures.total",
            description: "Total Keycloak API call failures");
            
        _keycloakApiDurations = meter.CreateHistogram<long>(
            "datacat.keycloak.api.duration.ms",
            unit: "ms",
            description: "Keycloak API call duration");
    }
    
    // Authentication methods
    public void AddAuthenticationAttempt(string authType)
    {
        _authenticationAttempts.Add(1, new TagList { { "type", authType } });
    }
    
    public void AddAuthenticationFailure(string authType)
    {
        _authenticationFailures.Add(1, new TagList { { "type", authType } });
    }
    
    public void RecordAuthenticationDuration(string authType, long durationMs, bool isSuccess)
    {
        var tags = new TagList
        {
            { "type", authType },
            { "status", isSuccess ? "success" : "failed" }
        };
        _authenticationDurations.Record(durationMs, tags);
    }
    
    // Token methods
    public void AddTokenRequest(string tokenType)
    {
        _tokenRequests.Add(1, new TagList { { "type", tokenType } });
    }
    
    public void AddTokenCacheHit(string tokenType)
    {
        _tokenCacheHits.Add(1, new TagList { { "type", tokenType } });
    }
    
    public void RecordTokenRequestDuration(string tokenType, long durationMs, bool isSuccess)
    {
        var tags = new TagList
        {
            { "type", tokenType },
            { "status", isSuccess ? "success" : "failed" }
        };
        _tokenRequestDurations.Record(durationMs, tags);
    }
    
    // User synchronization methods
    public void AddUserSyncOperation()
    {
        _userSyncOperations.Add(1);
    }
    
    public void AddUsersSynced(int count)
    {
        _usersSynced.Add(count);
    }
    
    public void RecordUserSyncDuration(long durationMs, bool isSuccess)
    {
        _userSyncDurations.Record(durationMs, new TagList { { "status", isSuccess ? "success" : "failed" } });
    }
    
    public void AddUserSyncFailure()
    {
        _userSyncFailures.Add(1);
    }
    
    // Role synchronization methods
    public void AddRoleSyncOperation()
    {
        _roleSyncOperations.Add(1);
    }
    
    public void AddRolesSynced(int count)
    {
        _rolesSynced.Add(count);
    }
    
    public void RecordRoleSyncDuration(long durationMs, bool isSuccess)
    {
        _roleSyncDurations.Record(durationMs, new TagList { { "status", isSuccess ? "success" : "failed" } });
    }
    
    public void AddRoleSyncFailure()
    {
        _roleSyncFailures.Add(1);
    }
    
    // HTTP request methods
    public void AddKeycloakApiCall(string endpoint)
    {
        _keycloakApiCalls.Add(1, new TagList { { "endpoint", endpoint } });
    }
    
    public void AddKeycloakApiFailure(string endpoint)
    {
        _keycloakApiFailures.Add(1, new TagList { { "endpoint", endpoint } });
    }
    
    public void RecordKeycloakApiDuration(string endpoint, long durationMs, bool isSuccess)
    {
        var tags = new TagList
        {
            { "endpoint", endpoint },
            { "status", isSuccess ? "success" : "failed" }
        };
        _keycloakApiDurations.Record(durationMs, tags);
    }
}