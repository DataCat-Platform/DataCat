namespace DataCat.Auth.Keycloak.Jobs;

[DisallowConcurrentExecution]
public sealed class UserSynchronizationJob(
    ILogger<UserSynchronizationJob> logger,
    HttpClient httpClient,
    IUserRepository userRepository,
    IJwtService jwtService,
    IUnitOfWork<IDbTransaction> unitOfWork)
    : BaseBackgroundWorker(logger)
{
    protected override string JobName => nameof(UserSynchronizationJob);

    protected override async Task RunAsync(CancellationToken stoppingToken = default)
    {
        var tokenResult = await jwtService.GetServerAccessTokenAsync(stoppingToken);

        if (tokenResult.IsFailure)
        {
            logger.LogError("[{Job}] Error was occured. Error: {@Errors}", nameof(UserSynchronizationJob), tokenResult.Errors);
            return;
        }
        
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/admin/realms/datacat/users?first={0}&max={1_000}");
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

            var users = JsonSerializer.Deserialize<List<UserKeycloakResponse>>(content);

            if (users is null)
            {
                logger.LogWarning("[{Job}] No users were returned from Keycloak.", nameof(UserSynchronizationJob));
                return;
            }

            logger.LogInformation("[{Job}] Successfully fetched {Count} users.", nameof(UserSynchronizationJob), users.Count);

            var userEntities = users.Select(user => UserEntity.Create(
                Guid.NewGuid(),
                user.IdentityId,
                user.Email,
                user.Name,
                DateTime.UtcNow,
                null,
                [],
                [])).ToList();

            var userErrors = userEntities.Where(x => x.IsFailure).ToList();

            if (userErrors.Count != 0)
            {
                foreach (var user in userErrors)
                {
                    logger.LogWarning("[{Job}] User cant be added to the system. Error: {@Errors}",
                        nameof(UserSynchronizationJob), user.Errors);
                }
            }

            await unitOfWork.StartTransactionAsync(stoppingToken);
            
            await userRepository.BulkInsertAsync(userEntities
                .Where(x => x.IsSuccess)
                .Select(x => x.Value)
                .ToList(), 
                stoppingToken);
            
            await unitOfWork.CommitAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Job}] Exception occurred during user synchronization.", nameof(UserSynchronizationJob));
        }
    }
}