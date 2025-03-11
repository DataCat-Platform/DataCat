namespace DataCat.Notifications.Telegram;

public sealed class TelegramPlugin : INotificationPlugin
{
    public IServiceCollection RegisterNotificationDestinationLibrary(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<INotificationOptionFactory, TelegramNotificationOptionFactory>();

        return services;
    }
}