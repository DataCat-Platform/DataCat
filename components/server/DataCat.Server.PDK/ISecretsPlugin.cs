namespace DataCat.Server.PDK;

public interface ISecretsPlugin
{
    IServiceCollection RegisterSecretsStorage(IServiceCollection services, IConfiguration configuration);
}