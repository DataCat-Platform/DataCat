namespace DataCat.Server.Application.Auth;

public interface IIdentity
{
    string Id { get; }
    
    IReadOnlyCollection<UserRole> Roles { get; } // RBAC
    
    // TODO: implement in the future versions
    // IReadOnlyCollection<string> Permissions { get; } // PBAC
    // IReadOnlyDictionary<string, string> Claims { get; } // Additional claims
    
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    bool IsViewer { get; }
    bool IsEditor { get; }
}