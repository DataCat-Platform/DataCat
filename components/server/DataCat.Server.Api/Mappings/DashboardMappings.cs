namespace DataCat.Server.Api.Mappings;

public static class DashboardMappings
{
    public static HomeSearchDashboardResponse ToResponse(this DashboardEntity dashboard)
    {
        return new HomeSearchDashboardResponse
        {
            DashboardId = dashboard.Id,
            Name = dashboard.Name,
            Description = dashboard.Description,
            OwnerId = dashboard.Owner.Id,
            UpdatedAt = dashboard.UpdatedAt
        };
    }
}