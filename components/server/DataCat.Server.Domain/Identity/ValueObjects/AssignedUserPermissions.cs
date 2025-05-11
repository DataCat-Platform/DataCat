namespace DataCat.Server.Domain.Identity.ValueObjects;

public sealed class AssignedUserPermissions : ValueObject
{
    public AssignedUserPermissions(UserPermission permission, Guid namespaceId, bool isManual)
    {
        Permission = permission;
        NamespaceId = namespaceId;
        IsManual = isManual;
    }
    
    public UserPermission Permission { get; }
    public Guid NamespaceId { get; }
    public bool IsManual { get; }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Permission;
        yield return NamespaceId;
        yield return IsManual;
    }
}