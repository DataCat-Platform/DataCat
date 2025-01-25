namespace DataCat.Server.Application.Queries.Plugin.GetAll;

public sealed record GetAllPluginsQuery : IRequest<Result<IEnumerable<PluginEntity>>>;
