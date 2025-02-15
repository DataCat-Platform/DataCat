namespace DataCat.Server.HttpModels.Requests.Plugin;

public class AddPluginRequest
{
    public required IFormFile File { get; set; }
    
    public required string Name { get; set; }

    public required string Version { get; set; }

    public string? Description { get; set; }

    public required string Author { get; set; }

    public string? Settings { get; set; }
}