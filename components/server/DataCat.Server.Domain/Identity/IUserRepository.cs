namespace DataCat.Server.Domain.Identity;

public interface IUserRepository
{
    Task DeleteAsync(Guid id, CancellationToken token = default);
}