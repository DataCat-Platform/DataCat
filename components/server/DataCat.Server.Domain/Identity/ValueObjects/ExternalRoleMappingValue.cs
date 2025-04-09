namespace DataCat.Server.Domain.Identity.ValueObjects;

public sealed class ExternalRoleMappingValue : ValueObject
{
    public ExternalRoleMappingValue(string externalRole, UserRole role, Guid namespaceId)
    {
        ExternalRole = externalRole;
        Role = role;
        NamespaceId = namespaceId;
    }
    
    public string ExternalRole { get; }
    public UserRole Role { get; }
    public Guid NamespaceId { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ExternalRole;
        yield return Role;
        yield return NamespaceId;
    }
}