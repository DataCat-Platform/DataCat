namespace DataCat.Server.Infrastructure.Auth;

public sealed class DefaultIdentity : IIdentity
{
    public required string Id { get; init; }
    
    public required IReadOnlyCollection<UserRole> Roles { get; init; }
    
    public bool IsAuthenticated => Roles.Count != 0;
    
    public bool IsAdmin => Roles.Any(x => x == UserRole.Admin);
    public bool IsViewer => Roles.Any(x => x == UserRole.Viewer);
    public bool IsEditor => Roles.Any(x => x == UserRole.Editor);
}