namespace DataCat.Server.PDK;

public interface IAuthPlugin
{
    IServiceCollection RegisterAuthExternalProvider(IServiceCollection services, IConfiguration configuration);
}