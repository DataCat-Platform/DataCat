namespace DataCat.Server.Domain.Core.Services;

public interface INotificationService
{
    Task SendNotificationAsync(AlertEntity alertEntity, CancellationToken token = default);
}
