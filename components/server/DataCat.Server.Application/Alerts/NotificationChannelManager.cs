namespace DataCat.Server.Application.Alerts;

public sealed class NotificationChannelManager(
    IServiceProvider serviceProvider,
    IEnumerable<INotificationOptionFactory> factories)
{
    private readonly ConcurrentDictionary<string, INotificationOptionFactory> _notificationOptionFactoryCache = new();

    public INotificationOptionFactory GetNotificationChannelFactory(NotificationDestination notificationDestination)
    {
        var key = notificationDestination.Name;

        if (_notificationOptionFactoryCache.TryGetValue(key, out var factory))
        {
            return factory;
        }

        var notificationFactory = factories.FirstOrDefault(x => x.NotificationDestination == notificationDestination);
        _notificationOptionFactoryCache[key] = notificationFactory 
                                               ?? throw new InvalidOperationException($"The Notification Destination {notificationDestination.Name} is not supported.");

        return notificationFactory;
    }
}