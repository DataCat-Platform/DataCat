namespace DataCat.Server.Application.Commands.Plugin.Update;

public sealed record UpdatePluginCommand : IRequest<Result>
{
    public required string PluginId { get; init; }
    
    public required string? Description { get; init; }

    public required string? Settings { get; init; }
}