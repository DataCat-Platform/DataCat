namespace DataCat.Server.Application.Alerts;

public abstract class BaseNotificationInitializer(IServiceProvider serviceProvider, ILogger logger) : IHostedService
{
    protected abstract Task ExecuteAsync(IServiceProvider scopedProvider, CancellationToken cancellationToken);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        const int maxAttempts = 3;
        const int delayMilliseconds = 100;
        var random = new Random();

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                await ExecuteAsync(scope.ServiceProvider, cancellationToken);

                return;
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                logger.LogWarning(ex, "[{Job}] Attempt {Attempt} failed. Retrying...", GetType().Name, attempt);
                await Task.Delay(delayMilliseconds * random.Next(1, 10), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{Job}] All {MaxAttempts} attempts failed.", GetType().Name, maxAttempts);
                throw;
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}