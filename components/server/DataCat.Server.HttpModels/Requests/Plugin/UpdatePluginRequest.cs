namespace DataCat.Server.HttpModels.Requests.Plugin;

public class UpdatePluginRequest
{
    public string? Description { get; init; }

    public string? Settings { get; init; }
}