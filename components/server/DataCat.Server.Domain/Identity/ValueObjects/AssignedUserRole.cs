namespace DataCat.Server.Domain.Identity.ValueObjects;

public sealed class AssignedUserRole : ValueObject
{
    public AssignedUserRole(UserRole role, Guid namespaceId, bool isManual)
    {
        Role = role;
        NamespaceId = namespaceId;
        IsManual = isManual;
    }
    
    public UserRole Role { get; }
    public Guid NamespaceId { get; }
    public bool IsManual { get; }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Role;
        yield return NamespaceId;
        yield return IsManual;
    }
}