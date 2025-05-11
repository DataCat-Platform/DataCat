namespace DataCat.Server.Domain.Core.Errors;

public sealed class DataSourceTypeError(string code, string message) : BaseError(code, message)
{
    public static DataSourceTypeError NotFound(string name) => new("DataSourceType.NotFound", $"DataSourceType with name {name} is not found.");
}