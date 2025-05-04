namespace DataCat.Server.Application.Commands.Plugins.ToggleStatus;

public sealed record ToggleStatusCommand : ICommand, IAdminRequest
{
    public required string PluginId { get; init; }
    
    public required ToggleStatus? ToggleStatus { get; init; }
}

public enum ToggleStatus
{
    Active,
    InActive,
}