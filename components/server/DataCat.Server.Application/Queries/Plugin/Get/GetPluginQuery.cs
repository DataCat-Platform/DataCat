namespace DataCat.Server.Application.Queries.Plugin.Get;

public sealed record GetPluginQuery(string PluginId) : IRequest<Result<PluginEntity>>;
