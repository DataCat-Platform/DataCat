namespace DataCat.Server.Domain.Core;

public sealed class Namespace
{
    private Namespace(
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
    private List<Guid> _dashboardIds { get; }
    public IReadOnlyCollection<Guid> DashboardIds => _dashboardIds.AsReadOnly();

    public static Result<Namespace> Create(
        Guid id,
        string name,
        IEnumerable<Guid>? dashboardIds)
    {
        var validationList = new List<Result<Namespace>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<Namespace>(BaseError.FieldIsNull(nameof(name))));
        }

        #endregion
        
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new Namespace(id, name, dashboardIds ?? []));
    }
}