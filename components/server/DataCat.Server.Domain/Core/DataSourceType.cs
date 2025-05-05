namespace DataCat.Server.Domain.Core;

public sealed class DataSourceType
{
    private DataSourceType(
        string name,
        int? id = null)
    {
        Id = id ?? 0;
        Name = name;
    }
    
    public int Id { get; }
    public string Name { get; private set; }

    public static Result<DataSourceType> Create(
        string name,
        int? id = null)
    {
        var validationList = new List<Result<DataSourceType>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<DataSourceType>(BaseError.FieldIsNull(nameof(name))));
        }

        #endregion
        
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new DataSourceType(name.ToLowerInvariant(), id));
    }
}