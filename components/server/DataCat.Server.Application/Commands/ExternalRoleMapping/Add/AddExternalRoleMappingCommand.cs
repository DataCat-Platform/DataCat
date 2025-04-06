namespace DataCat.Server.Application.Commands.ExternalRoleMapping.Add;

public sealed record AddExternalRoleMappingCommand(
    string ExternalRole,
    string? NamespaceId,
    int RoleId) : IRequest<Result>;
