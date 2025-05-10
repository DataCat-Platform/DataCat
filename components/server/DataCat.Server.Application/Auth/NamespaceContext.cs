namespace DataCat.Server.Application.Auth;

public sealed class NamespaceContext
{
    public string NamespaceId { get; set; } = ApplicationConstants.DefaultNamespaceId;
    
    public Guid GetNamespaceId() => Guid.Parse(NamespaceId);
}