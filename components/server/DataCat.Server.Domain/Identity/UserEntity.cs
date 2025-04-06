namespace DataCat.Server.Domain.Identity;

public class UserEntity
{
    private List<AssignedUserRole> _roles;
    private List<AssignedUserPermissions> _permissions;
    
    private UserEntity(
        Guid id, 
        string identityId,
        string email, 
        string name,
        DateTime createdAt,
        DateTime? updatedAt,
        IEnumerable<AssignedUserRole> roles,
        IEnumerable<AssignedUserPermissions> permissions)
    {
        Id = id;
        IdentityId = identityId;
        Email = email;
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _roles = roles.ToList();
        _permissions = permissions.ToList();
    }

    public Guid Id { get; private set; }
    public string IdentityId { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<AssignedUserRole> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<AssignedUserPermissions> Permissions => _permissions.AsReadOnly();

    public static Result<UserEntity> Create(
        Guid id,
        string identityId,
        string email,
        string name,
        DateTime createdAt,
        DateTime? updatedAt,
        IEnumerable<AssignedUserRole> roles,
        IEnumerable<AssignedUserPermissions> permissions)
    {
        // todo: add validation
        
        return Result.Success(new UserEntity(id, identityId, email, name, createdAt, updatedAt, roles, permissions));
    }
}