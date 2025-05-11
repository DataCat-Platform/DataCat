namespace DataCat.Notifications.Webhook;

public sealed class WebhookPlugin : INotificationPlugin
{
    public IServiceCollection RegisterNotificationDestinationLibrary(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<INotificationOptionFactory, WebhookNotificationOptionFactory>();

        services.AddHostedService<WebhookNotificationInitializer>();
        
        return services;
    }
}