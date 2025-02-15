namespace DataCat.Server.Domain.Core.Errors;

public sealed class DataSourceError(string code, string message) : BaseError(code, message)
{
    public static DataSourceError NotFound(string id) => new("DataSource.NotFound", $"DataSource with id {id} is not found.");
}