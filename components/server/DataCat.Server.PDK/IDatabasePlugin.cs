namespace DataCat.Server.PDK;

public interface IDatabasePlugin
{
    IServiceCollection RegisterRepositories(IServiceCollection services, IConfiguration configuration);
}