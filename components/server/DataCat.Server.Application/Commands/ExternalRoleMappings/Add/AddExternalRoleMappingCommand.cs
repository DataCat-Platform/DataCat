namespace DataCat.Server.Application.Commands.ExternalRoleMappings.Add;

public sealed record AddExternalRoleMappingCommand(
    string ExternalRole,
    string? NamespaceId,
    int RoleId) : ICommand, IAdminRequest;
