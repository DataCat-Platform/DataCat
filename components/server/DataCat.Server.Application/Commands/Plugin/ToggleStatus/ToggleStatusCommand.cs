namespace DataCat.Server.Application.Commands.Plugin.ToggleStatus;

public sealed record ToggleStatusCommand : IRequest<Result>, ITransactionScopeRequest, IAdminRequest
{
    public required string PluginId { get; init; }
    
    public required ToggleStatus? ToggleStatus { get; init; }
}

public enum ToggleStatus
{
    Active,
    InActive,
}