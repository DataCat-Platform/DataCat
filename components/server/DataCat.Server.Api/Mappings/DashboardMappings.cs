namespace DataCat.Server.Api.Mappings;

public static class DashboardMappings
{
    public static AddDashboardCommand ToAddCommand(this AddDashboardRequest request)
    {
        return new AddDashboardCommand
        {
            Name = request.Name,
            Description = request.Description,
            UserId = request.UserId
        };
    }

    public static UpdateDashboardCommand ToUpdateCommand(this UpdateDashboardRequest request, string dashboardId)
    {
        return new UpdateDashboardCommand
        {
            DashboardId = dashboardId,
            Name = request.Name,
            Description = request.Description
        };
    }

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
    
    public static OverviewDashboardResponse ToFullResponse(this DashboardEntity dashboard)
    {
        return new OverviewDashboardResponse
        {
            DashboardId = dashboard.Id,
            Name = dashboard.Name,
            Description = dashboard.Description,
            OwnerId = dashboard.Owner.Id,
            UpdatedAt = dashboard.UpdatedAt,
            Panels = dashboard?.Panels.Select(x => new PanelDetailsDto
            {
                Id = x.Id,
                PanelType = x.PanelType.Name,
                Query = x.QueryEntity.RawQuery,
                Title = x.Title,
                Layout = new DataCatLayoutDto()
                {
                    X = x.DataCatLayout.X,
                    Y = x.DataCatLayout.Y,
                    Width = x.DataCatLayout.Width,
                    Height = x.DataCatLayout.Height,
                }
            }).ToList(),
            Users = dashboard?.SharedWith.Select(x => new UserResponse
            {
                UserId = x.Id,
                UserName = x.Username,
                Role = x.Role.Name,
                Email = x.Email,
            })
        };
    }

    public static AddUserToDashboardCommand ToCommand(this AddUserToDashboardRequest request)
    {
        return new AddUserToDashboardCommand()
        {
            UserId = request.UserId,
            DashboardId = request.DashboardId,
        };
    } 
}