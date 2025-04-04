namespace DataCat.Server.Infrastructure.Auth;

public sealed class DefaultIdentityProvider(AuthMappingOptions _)
    : IIdentityProvider
{
    public IIdentity? CurrentIdentity { get; private set; }
    
    public Task LoadIdentityAsync(CancellationToken token = default)
    {
        CurrentIdentity = new DefaultIdentity
        {
            Id = "Default-Identity",
            Roles = [UserRole.Admin, UserRole.Viewer, UserRole.Editor]
        };
        return Task.CompletedTask;
    }
}