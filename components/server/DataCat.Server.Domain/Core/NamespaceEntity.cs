namespace DataCat.Server.Domain.Core;

public sealed class NamespaceEntity
{
    private List<Guid> _dashboardIds { get; }

    private NamespaceEntity(
        Guid id,
        string name,
        IEnumerable<Guid> dashboardIds)
    {
        Id = id;
        Name = name;
        _dashboardIds = dashboardIds.ToList();
    }
    
    public Guid Id { get; }
    public string Name { get; private set; }

    public IReadOnlyCollection<Guid> DashboardIds => _dashboardIds.AsReadOnly();

    public static Result<NamespaceEntity> Create(
        Guid id,
        string name,
        IEnumerable<Guid>? dashboardIds)
    {
        var validationList = new List<Result<NamespaceEntity>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<NamespaceEntity>(BaseError.FieldIsNull(nameof(name))));
        }

        #endregion
        
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new NamespaceEntity(id, name, dashboardIds ?? []));
    }
}