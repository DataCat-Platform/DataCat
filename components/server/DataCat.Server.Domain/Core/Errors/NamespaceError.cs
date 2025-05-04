namespace DataCat.Server.Domain.Core.Errors;

public sealed class NamespaceError(string code, string message) : BaseError(code, message)
{
    public static NamespaceError NotFound(string name) => new("Namespace.NotFound", $"Namespace with name {name} is not found.");
}