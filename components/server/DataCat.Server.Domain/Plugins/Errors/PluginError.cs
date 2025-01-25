namespace DataCat.Server.Domain.Plugins.Errors;

public sealed class PluginError(string code, string message) : BaseError(code, message)
{
    public static readonly PluginError InvalidToggleStatus = new("Plugin.Status", "Invalid toggle status. It should be 'Active' or 'Inactive'");
    public static readonly PluginError ErrorDuringUpdate = new("Plugin.Database", "Error during update the plugin enable/disable status");
    
    public static PluginError NotFound(string id) => new("Plugin.NotFound", $"Plugin with id {id} is not found.");
}