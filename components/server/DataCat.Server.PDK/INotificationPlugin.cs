namespace DataCat.Server.PDK;

public interface INotificationPlugin
{
    IServiceCollection RegisterNotificationDestinationLibrary(IServiceCollection services, IConfiguration configuration);
}
