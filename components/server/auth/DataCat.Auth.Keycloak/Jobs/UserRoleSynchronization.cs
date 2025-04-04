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
        await Task.CompletedTask;
    }
}