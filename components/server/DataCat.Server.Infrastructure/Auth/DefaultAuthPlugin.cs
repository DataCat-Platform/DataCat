namespace DataCat.Server.Infrastructure.Auth;

public sealed class DefaultAuthPlugin : IAuthPlugin
{
    public IServiceCollection RegisterAuthExternalProvider(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}