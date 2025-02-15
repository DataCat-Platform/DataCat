namespace DataCat.Server.Domain.Core;

public class DashboardEntity
{
    private DashboardEntity(
        Guid id,
        string name,
        string? description,
        IEnumerable<PanelEntity> panels,
        UserEntity owner,
        IEnumerable<UserEntity> sharedWith,
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

    public string? Description { get; private set; }

    public IEnumerable<PanelEntity> Panels { get; private set; }

    public UserEntity Owner { get; private set; }

    public IEnumerable<UserEntity> SharedWith { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
    
    public void ChangeName(string name) => Name = name;
    
    public void ChangeDescription(string? description) => Description = description;
    
    public static Result<DashboardEntity> Create(
        Guid id,
        string name,
        string? description,
        IEnumerable<PanelEntity>? panels,
        UserEntity? owner,
        IEnumerable<UserEntity>? sharedWith,
        DateTime createdAt,
        DateTime updatedAt)
    {
        var validationList = new List<Result<DashboardEntity>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<DashboardEntity>(BaseError.FieldIsNull(nameof(name))));
        }

        if (owner is null)
        {
            validationList.Add(Result.Fail<DashboardEntity>(BaseError.FieldIsNull(nameof(owner))));
        }

        #endregion

        panels ??= Array.Empty<PanelEntity>();
        sharedWith ??= Array.Empty<UserEntity>();

        return validationList.Count != 0
            ? validationList.FoldResults()!
            : Result.Success(new DashboardEntity(
                id,
                name,
                description,
                panels,
                owner!,
                sharedWith,
                createdAt,
                updatedAt));
    }
}