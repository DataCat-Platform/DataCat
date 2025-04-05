namespace DataCat.Auth.Keycloak.Jobs;

[DisallowConcurrentExecution]
public sealed class UserRoleSynchronizationJob(
    ILogger<UserSynchronizationJob> logger,
    HttpClient httpClient,
    IUserRepository userRepository,
    IJwtService jwtService,
    IUnitOfWork<IDbTransaction> unitOfWork)
    : BaseBackgroundWorker(logger)
{
    protected override string JobName => nameof(UserRoleSynchronizationJob);
    
    protected override async Task RunAsync(CancellationToken stoppingToken = default)
    {
        await unitOfWork.StartTransactionAsync(stoppingToken);
        
        var user = await userRepository.GetOldestByUpdatedAtUserAsync(stoppingToken);

        if (user is null)
        {
            logger.LogWarning("[{Job}] No stale users were found to process", nameof(UserRoleSynchronizationJob));
            return;
        }
        
        var tokenResult = await jwtService.GetServerAccessTokenAsync(stoppingToken);

        if (tokenResult.IsFailure)
        {
            logger.LogError("[{Job}] Error was occured. Error: {@Errors}", nameof(UserRoleSynchronizationJob), tokenResult.Errors);
            return;
        }
        
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/admin/realms/datacat/users/{user.IdentityId}/role-mappings");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Value);

        try
        {
            using var response = await httpClient.SendAsync(request, stoppingToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("[{Job}] Request failed with status code {StatusCode}. Reason: {ReasonPhrase}",
                    nameof(UserSynchronizationJob), response.StatusCode, response.ReasonPhrase);
                return;
            }

            var content = await response.Content.ReadAsStringAsync(stoppingToken);
            var realmMappings = JsonSerializer.Deserialize<RealmMappingsResponse>(content);

            if (realmMappings is null)
            {
                logger.LogError("[{Job}] Invalid response from Keycloak", nameof(UserRoleSynchronizationJob));
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

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Job}] Exception occurred during user synchronization.", nameof(UserRoleSynchronizationJob));
        }
        finally
        {
            await unitOfWork.RollbackAsync(stoppingToken);
        }
    }
}