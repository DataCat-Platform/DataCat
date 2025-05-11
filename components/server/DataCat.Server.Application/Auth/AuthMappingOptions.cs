namespace DataCat.Server.Application.Auth;

public sealed class AuthMappingOptions
{
    public required Dictionary<string, string> RoleMappings { get; init; }

    public UserRole? MapExternalRole(string externalRole)
    {
        var domainRole = RoleMappings.GetValueOrDefault(externalRole);
        return domainRole is null 
            ? null 
            : UserRole.FromName(domainRole);
    }
}