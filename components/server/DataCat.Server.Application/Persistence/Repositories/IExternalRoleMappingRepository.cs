namespace DataCat.Server.Application.Persistence.Repositories;

public interface IExternalRoleMappingRepository
{
    Task AddExternalRoleMappingAsync(
        ExternalRoleMappingValue externalRoleMappingValue, 
        CancellationToken token = default);

    Task<List<ExternalRoleMappingValue>> GetExternalRoleMappingsByExternalRolesAsync(
        string[] externalRoles,
        CancellationToken token = default);
}