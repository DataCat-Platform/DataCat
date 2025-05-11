namespace DataCat.Server.Domain.Core.Errors;

public sealed class AlertError(string code, string message) : BaseError(code, message)
{
    public static readonly AlertError NegativeWidth = new("AlertError.Measurements", "Width must be greater than 0");
    public static readonly AlertError NegativeHeight = new("AlertError.Measurements", "Height must be greater than 0");
    public static readonly AlertError InvalidNextExecutionTime = new("AlertError.Schedule", "Current execution time must be smaller than transmitted");
    
    public static DashboardError NotFound(string id) => new("Alert.NotFound", $"Alert with id {id} is not found.");
    public static DashboardError NotFound(Guid id) => NotFound(id.ToString());
}