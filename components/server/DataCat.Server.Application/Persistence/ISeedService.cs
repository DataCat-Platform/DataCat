namespace DataCat.Server.Application.Persistence;

public interface ISeedService
{
    Task SeedAsync(CancellationToken token = default);
}