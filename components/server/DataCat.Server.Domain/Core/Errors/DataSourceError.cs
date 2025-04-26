namespace DataCat.Server.Domain.Core.Errors;

public sealed class DataSourceError(string code, string message) : BaseError(code, message)
{
    public static DataSourceError NotFoundById(string id) => new("DataSource.NotFound", $"DataSource with id {id} is not found.");
    public static DataSourceError NotFoundByName(string name) => new("DataSource.NotFound", $"DataSource with name {name} is not found.");
}