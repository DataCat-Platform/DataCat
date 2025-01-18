namespace DataCat.Server.Domain.Plugins;

public class Plugin
{
    private Plugin(
        string pluginHash,
        string name,
        string version,
        string description,
        string author,
        bool isEnabled,
        string settings,
        DateTime createdAt,
        DateTime updatedAt,
        DateTime? lastLoadedAt)
    {
        PluginHash = pluginHash;
        Name = name;
        Version = version;
        Description = description;
        Author = author;
        IsEnabled = isEnabled;
        Settings = settings;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        LastLoadedAt = lastLoadedAt;
    }

    public string PluginHash { get; private set; }

    public string Name { get; private set; }

    public string Version { get; private set; }

    public string Description { get; private set; }

    public string Author { get; private set; }

    public bool IsEnabled { get; private set; }

    public string Settings { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public DateTime? LastLoadedAt { get; private set; }

    public static Result<Plugin> Create(
        string pluginHash,
        string name,
        string version,
        string description,
        string author,
        bool isEnabled,
        string settings,
        DateTime createdAt,
        DateTime updatedAt,
        DateTime? lastLoadedAt)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Fail<Plugin>("Name cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            return Result.Fail<Plugin>("Version cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return Result.Fail<Plugin>("Description cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            return Result.Fail<Plugin>("Author cannot be null or empty");
        }

        return Result.Success(new Plugin(
            pluginHash,
            name,
            version,
            description,
            author,
            isEnabled,
            settings,
            createdAt,
            updatedAt,
            lastLoadedAt));
    }
}