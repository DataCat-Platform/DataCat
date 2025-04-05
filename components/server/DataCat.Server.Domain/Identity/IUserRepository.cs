namespace DataCat.Server.Domain.Identity;

public interface IUserRepository
{
    Task DeleteAsync(Guid id, CancellationToken token = default);
    
    Task BulkInsertAsync(IList<UserEntity> users, CancellationToken token = default);
    
    Task<UserEntity?> FindByEmailAsync(string email, CancellationToken token = default);
    
    /// <summary>
    /// Todo: implement with passing external_role_param to fetch only necessary roles
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<List<ExternalRoleMapping>> GetExternalRoleMappingsAsync(CancellationToken token = default);
    
    Task<UserEntity?> GetOldestByUpdatedAtUserAsync(CancellationToken token = default);
    
    Task UpdateUserRolesAsync(UserEntity user, List<ExternalRoleMapping> currentUserRolesFromKeycloak, CancellationToken token);
}