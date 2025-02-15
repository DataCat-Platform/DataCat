namespace DataCat.Server.HttpModels.Requests.Dashboard;

public class UpdateDashboardRequest
{
    public required string Name { get; init; }
    
    public string? Description { get; init; }
}