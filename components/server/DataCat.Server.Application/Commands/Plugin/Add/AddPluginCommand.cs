namespace DataCat.Server.Application.Commands.Plugin.Add;

public sealed record AddPluginCommand : IRequest<Result<Guid>>
{
    public required IFormFile File { get; init; }
    
    public required string Name { get; init; }

    public required string Version { get; init; }

    public string? Description { get; init; }

    public string? Author { get; init; }

    public string? Settings { get; init; }
}