namespace DataCat.Server.Domain.Core;

public sealed class NotificationDestination
{
    private NotificationDestination(
        string name,
        int? id = null)
    {
        Id = id ?? 0;
        Name = name;
    }
    
    public int Id { get; }
    public string Name { get; private set; }

    public static Result<NotificationDestination> Create(
        string name,
        int? id = null)
    {
        var validationList = new List<Result<NotificationDestination>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<NotificationDestination>(BaseError.FieldIsNull(nameof(name))));
        }

        #endregion
        
        return validationList.Count != 0
            ? validationList.FoldResults()! 
            : Result.Success(new NotificationDestination(name.ToLowerInvariant(), id));
    }
}