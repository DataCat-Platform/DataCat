namespace DataCat.Server.Application.Commands.Plugins.Remove;

public sealed record RemovePluginCommand(string PluginId) : ICommand, IAdminRequest;
