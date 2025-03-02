namespace DataCat.Server.Application.Auth;

public interface IIdentityProvider
{
    IIdentity? CurrentIdentity { get; }
    
    Task LoadIdentityAsync(CancellationToken token = default); 
}