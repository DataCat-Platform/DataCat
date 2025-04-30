namespace DataCat.Server.Domain.Plugins;

public class Plugin
{
    private Plugin(
        Guid pluginId,
        string name,
        string version,
        string? description,
        string author,
        bool isEnabled,
        string? settings,
        DateTime createdAt,
        DateTime updatedAt)
    {
        PluginId = pluginId;
        Name = name;
        Version = version;
        Description = description;
        Author = author;
        IsEnabled = isEnabled;
        Settings = settings;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid PluginId { get; private set; }

    public string Name { get; private set; }

    public string Version { get; private set; }

    public string? Description { get; private set; }

    public string Author { get; private set; }

    public bool IsEnabled { get; private set; }

    public string? Settings { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public void LoadConfiguration(string? description, string? settings)
    {
        Description = description;
        Settings = settings;
    }

    public static Result<Plugin> Create(
        Guid pluginId,
        string name,
        string version,
        string? description,
        string? author,
        bool isEnabled,
        string? settings,
        DateTime createdAt,
        DateTime updatedAt)
    {
        var validationList = new List<Result<Plugin>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<Plugin>(BaseError.FieldIsNull(nameof(name))));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            validationList.Add(Result.Fail<Plugin>(BaseError.FieldIsNull(nameof(version))));
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            validationList.Add(Result.Fail<Plugin>(BaseError.FieldIsNull(nameof(author))));
        }

        #endregion

        return validationList.Count != 0
            ? validationList.FoldResults()!
            : Result.Success(new Plugin(
                pluginId,
                name,
                version,
                description,
                author!,
                isEnabled,
                settings,
                createdAt,
                updatedAt));
    }
}