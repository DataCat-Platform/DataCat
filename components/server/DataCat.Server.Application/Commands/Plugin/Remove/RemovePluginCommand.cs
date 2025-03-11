namespace DataCat.Server.Application.Commands.Plugin.Remove;

public sealed record RemovePluginCommand(string PluginId) : IRequest<Result>, IAdminRequest;
