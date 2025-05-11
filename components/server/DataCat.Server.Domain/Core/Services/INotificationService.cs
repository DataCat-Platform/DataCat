namespace DataCat.Server.Domain.Core.Services;

public interface INotificationService
{
    Task SendNotificationAsync(Alert alert, CancellationToken token = default);
}
