namespace DataCat.Server.Application.Commands.Plugin.ToggleStatus;

public sealed record ToggleStatusCommand : IRequest<Result>, ITransactionScopeRequest
{
    public required string PluginId { get; set; }
    
    public required ToggleStatus? ToggleStatus { get; set; }
}

public enum ToggleStatus
{
    Active,
    InActive,
}