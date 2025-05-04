namespace DataCat.Auth.Keycloak.Jobs;

[DisallowConcurrentExecution]
public sealed class UserRoleSynchronizationJob(
    ILogger<UserSynchronizationJob> logger,
    HttpClient httpClient,
    IUserRepository userRepository,
    IJwtService jwtService,
    IUnitOfWork<IDbTransaction> unitOfWork,
    IMetricsContainer metricsContainer,
    KeycloakMetricsContainer keycloakMetricsContainer)
    : BaseBackgroundWorker(logger, metricsContainer)
{
    private const string RoleMappingsKeycloakEndpoint = "role-mappings";
    
    protected override string JobName => nameof(UserRoleSynchronizationJob);
    
    protected override async Task RunAsync(CancellationToken stoppingToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        keycloakMetricsContainer.AddRoleSyncOperation();
        
        await unitOfWork.StartTransactionAsync(stoppingToken);
        
        var user = await userRepository.GetOldestByUpdatedAtUserAsync(stoppingToken);

        if (user is null)
        {
            logger.LogWarning("[{Job}] No stale users were found to process", nameof(UserRoleSynchronizationJob));
            keycloakMetricsContainer.AddRoleSyncFailure();
            return;
        }
        
        var tokenResult = await jwtService.GetServerAccessTokenAsync(stoppingToken);

        if (tokenResult.IsFailure)
        {
            logger.LogError("[{Job}] Error was occured. Error: {@Errors}", nameof(UserRoleSynchronizationJob), tokenResult.Errors);
            keycloakMetricsContainer.AddRoleSyncFailure();
            return;
        }
        
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/admin/realms/datacat/users/{user.IdentityId}/role-mappings");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Value);

        try
        {
            keycloakMetricsContainer.AddKeycloakApiCall(RoleMappingsKeycloakEndpoint);
            
            var apiStopwatch = Stopwatch.StartNew();
            using var response = await httpClient.SendAsync(request, stoppingToken);
            apiStopwatch.Stop();
            
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("[{Job}] Request failed with status code {StatusCode}. Reason: {ReasonPhrase}",
                    nameof(UserSynchronizationJob), response.StatusCode, response.ReasonPhrase);
                
                keycloakMetricsContainer.AddKeycloakApiFailure(RoleMappingsKeycloakEndpoint);
                keycloakMetricsContainer.AddRoleSyncFailure(); 
                
                return;
            }

            var content = await response.Content.ReadAsStringAsync(stoppingToken);
            var realmMappings = JsonSerializer.Deserialize<RealmMappingsResponse>(content);

            if (realmMappings is null)
            {
                logger.LogError("[{Job}] Invalid response from Keycloak", nameof(UserRoleSynchronizationJob));
                keycloakMetricsContainer.AddKeycloakApiFailure(RoleMappingsKeycloakEndpoint);
                keycloakMetricsContainer.AddRoleSyncFailure();
                return;
            }

            var userRolesFromKeycloak = realmMappings.RealmMappings;

            var externalMappings = await userRepository.GetExternalRoleMappingsAsync(stoppingToken);

            // match current roles from keycloak and external roles from database
            var currentUserRolesFromKeycloak = externalMappings
                .Where(x =>
                    userRolesFromKeycloak.Any(y => y.Name == x.ExternalRole))
                .ToList();

            await userRepository.UpdateUserRolesAsync(user, currentUserRolesFromKeycloak, stoppingToken);

            await unitOfWork.CommitAsync(stoppingToken);
            
            var duration = stopwatch.ElapsedMilliseconds;
            keycloakMetricsContainer.AddRolesSynced(currentUserRolesFromKeycloak.Count);
            keycloakMetricsContainer.RecordRoleSyncDuration(duration, isSuccess: true);
            keycloakMetricsContainer.RecordKeycloakApiDuration("role-mappings", apiStopwatch.ElapsedMilliseconds, isSuccess: true);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Job}] Exception occurred during user synchronization.", nameof(UserRoleSynchronizationJob));
            await unitOfWork.RollbackAsync(stoppingToken);
            
            var duration = stopwatch.ElapsedMilliseconds;
            keycloakMetricsContainer.AddRoleSyncFailure();
            keycloakMetricsContainer.RecordRoleSyncDuration(duration, isSuccess: false);
        }
    }
}