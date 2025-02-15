namespace DataCat.Server.PDK;

public interface IDatabasePlugin
{
    void RegisterRepositories(IServiceCollection services, IConfiguration configuration);
}