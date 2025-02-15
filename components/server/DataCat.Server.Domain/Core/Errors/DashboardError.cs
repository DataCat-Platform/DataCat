namespace DataCat.Server.Domain.Core.Errors;

public sealed class DashboardError(string code, string message) : BaseError(code, message)
{
    public static DashboardError NotFound(string id) => new("Dashboard.NotFound", $"Dashboard with id {id} is not found.");
}