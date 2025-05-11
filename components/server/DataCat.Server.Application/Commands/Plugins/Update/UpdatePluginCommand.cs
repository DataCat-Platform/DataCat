namespace DataCat.Server.Application.Commands.Plugins.Update;

public sealed record UpdatePluginCommand : ICommand, IAdminRequest
{
    public required string PluginId { get; init; }
    
    public required string? Description { get; init; }

    public required string? Settings { get; init; }
}