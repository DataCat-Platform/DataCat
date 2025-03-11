namespace DataCat.Server.Application.Queries.Plugin.Get;

public sealed record GetPluginQuery(Guid PluginId) : IRequest<Result<PluginEntity>>, IAdminRequest;
