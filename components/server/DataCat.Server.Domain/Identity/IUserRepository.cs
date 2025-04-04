namespace DataCat.Server.Domain.Identity;

public interface IUserRepository
{
    Task DeleteAsync(Guid id, CancellationToken token = default);
    
    Task BulkInsertAsync(IList<UserEntity> users, CancellationToken token = default);
    
    Task<UserEntity?> FindByEmailAsync(string email, CancellationToken token = default);
}