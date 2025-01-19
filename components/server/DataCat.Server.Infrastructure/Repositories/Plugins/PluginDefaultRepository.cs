namespace DataCat.Server.Infrastructure.Repositories.Plugins;

public class PluginDefaultRepository : IDefaultRepository<Plugin, Guid>
{
    
    
    public Task<Plugin?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Plugin>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Plugin dataSource)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Plugin dataSource)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}