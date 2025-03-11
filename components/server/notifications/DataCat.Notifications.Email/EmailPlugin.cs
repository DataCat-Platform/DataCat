namespace DataCat.Notifications.Email;

public sealed class EmailPlugin : INotificationPlugin
{
    public IServiceCollection RegisterNotificationDestinationLibrary(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<INotificationOptionFactory, EmailNotificationOptionFactory>();

        return services;
    }
}