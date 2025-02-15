namespace DataCat.Server.HttpModels.Requests.Dashboard;

public class AddDashboardRequest
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public required string UserId { get; init; }
}