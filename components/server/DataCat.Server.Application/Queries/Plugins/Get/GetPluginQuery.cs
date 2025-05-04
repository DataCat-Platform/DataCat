namespace DataCat.Server.Application.Queries.Plugins.Get;

public sealed record GetPluginQuery(Guid PluginId)
    : IQuery<GetPluginResponse>, IAdminRequest;
