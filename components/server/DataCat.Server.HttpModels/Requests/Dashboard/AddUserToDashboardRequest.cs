namespace DataCat.Server.HttpModels.Requests.Dashboard;

public class AddUserToDashboardRequest
{
    public required string DashboardId { get; set; }

    public required string UserId { get; set; }
}