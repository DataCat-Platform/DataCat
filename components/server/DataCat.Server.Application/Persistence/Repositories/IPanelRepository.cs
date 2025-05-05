namespace DataCat.Server.Application.Persistence.Repositories;

public interface IPanelRepository
{
    Task UpdateAsync(Panel entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}