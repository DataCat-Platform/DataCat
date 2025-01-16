namespace DataCat.Server.Domain.Models;

public class Dashboard
{
    private Dashboard(
        Guid id,
        string name,
        string description,
        IEnumerable<Panel> panels,
        User owner,
        IEnumerable<User> sharedWith,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Panels = panels;
        Owner = owner;
        SharedWith = sharedWith;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public IEnumerable<Panel> Panels { get; private set; }

    public User Owner { get; private set; }

    public IEnumerable<User> SharedWith { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Result<Dashboard> Create(
        Guid id,
        string name,
        string description,
        IEnumerable<Panel>? panels,
        User? owner,
        IEnumerable<User>? sharedWith,
        DateTime createdAt,
        DateTime updatedAt)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Fail<Dashboard>("Name cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return Result.Fail<Dashboard>("Description cannot be null or empty");
        }

        if (owner is null)
        {
            return Result.Fail<Dashboard>("Owner cannot be null");
        }

        panels ??= Enumerable.Empty<Panel>();
        sharedWith ??= Enumerable.Empty<User>();

        return Result.Success(new Dashboard(
            id,
            name,
            description,
            panels,
            owner,
            sharedWith,
            createdAt,
            updatedAt));
    }
}