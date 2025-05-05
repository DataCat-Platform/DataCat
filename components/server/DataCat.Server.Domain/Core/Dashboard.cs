namespace DataCat.Server.Domain.Core;

public class Dashboard
{
    private Dashboard(
        Guid id,
        string name,
        string? description,
        IEnumerable<Panel> panels,
        User owner,
        IEnumerable<User> sharedWith,
        Guid namespaceId,
        DateTime createdAt,
        DateTime updatedAt,
        List<Tag> tags)
    {
        _tags = tags;
        Id = id;
        Name = name;
        Description = description;
        Panels = panels;
        Owner = owner;
        SharedWith = sharedWith;
        NamespaceId = namespaceId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public IEnumerable<Panel> Panels { get; private set; }

    public User Owner { get; private set; }

    public IEnumerable<User> SharedWith { get; private set; }
    public Guid NamespaceId { get; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
    
    private readonly List<Tag> _tags;
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    
    public void ChangeName(string name) => Name = name;
    
    public void ChangeDescription(string? description) => Description = description;
    
    public static Result<Dashboard> Create(
        Guid id,
        string name,
        string? description,
        IEnumerable<Panel>? panels,
        User? owner,
        IEnumerable<User>? sharedWith,
        Guid namespaceId,
        DateTime createdAt,
        DateTime updatedAt,
        List<Tag> tags)
    {
        var validationList = new List<Result<Dashboard>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<Dashboard>(BaseError.FieldIsNull(nameof(name))));
        }

        if (owner is null)
        {
            validationList.Add(Result.Fail<Dashboard>(BaseError.FieldIsNull(nameof(owner))));
        }
        
        #endregion

        panels ??= Array.Empty<Panel>();
        sharedWith ??= Array.Empty<User>();

        return validationList.Count != 0
            ? validationList.FoldResults()!
            : Result.Success(new Dashboard(
                id,
                name,
                description,
                panels,
                owner!,
                sharedWith,
                namespaceId,
                createdAt,
                updatedAt,
                tags));
    }
}