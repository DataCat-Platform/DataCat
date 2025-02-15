namespace DataCat.Server.Domain.Core.Errors;

public class PanelError(string code, string message) : BaseError(code, message)
{
    public static readonly PanelError InvalidPanelType = new("PanelError.NotFound", "Invalid panel type is not found.");
    
    public static PanelError NotFound(string id) => new("PanelError.NotFound", $"Panel with id {id} is not found.");
}