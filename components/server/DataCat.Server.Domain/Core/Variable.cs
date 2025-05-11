namespace DataCat.Server.Domain.Core;

public sealed class Variable
{
    private Variable(
        Guid id,
        string placeholder,
        string value,
        Guid namespaceId,
        Guid? dashboardId)
    {
        Id = id;
        Placeholder = placeholder;
        Value = value;
        NamespaceId = namespaceId;
        DashboardId = dashboardId;
    }
    
    public Guid Id { get; }
    public string Placeholder { get; }
    public string Value { get; }
    public Guid NamespaceId { get; }
    public Guid? DashboardId { get; }

    public static Result<Variable> Create(Guid id, string placeholder, string value, Guid namespaceId, Guid? dashboardId = null)
    {
        var validationList = new List<Result<Variable>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(placeholder))
        {
            validationList.Add(Result.Fail<Variable>(BaseError.FieldIsNull(nameof(placeholder))));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            validationList.Add(Result.Fail<Variable>(BaseError.FieldIsNull(nameof(value))));
        }

        #endregion

        return validationList.Count != 0
            ? validationList.FoldResults()!
            : Result.Success(new Variable(id, placeholder, value, namespaceId, dashboardId));
    }
}