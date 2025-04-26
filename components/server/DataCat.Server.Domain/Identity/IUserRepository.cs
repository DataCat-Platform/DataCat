namespace DataCat.Server.Domain.Identity;

public interface IUserRepository
{
    Task DeleteAsync(Guid id, CancellationToken token = default);
    
    Task BulkInsertAsync(IList<User> users, CancellationToken token = default);
    
    Task<User?> FindByEmailAsync(string email, CancellationToken token = default);
    
    /// <summary>
    /// Todo: implement with passing external_role_param to fetch only necessary roles
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<List<ExternalRoleMappingValue>> GetExternalRoleMappingsAsync(CancellationToken token = default);
    
    Task<User?> GetOldestByUpdatedAtUserAsync(CancellationToken token = default);
    
    Task UpdateUserRolesAsync(User user, List<ExternalRoleMappingValue> currentUserRolesFromKeycloak, CancellationToken token);
}