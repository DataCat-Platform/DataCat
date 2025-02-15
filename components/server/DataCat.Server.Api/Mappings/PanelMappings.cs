namespace DataCat.Server.Api.Mappings;

public static class PanelMappings
{
    public static AddPanelCommand ToAddCommand(this AddPanelRequest request)
    {
        return new AddPanelCommand
        {
            Title = request.Title,
            Type = request.Type,
            DashboardId = request.DashboardId,
            DataSourceId = request.DataSourceId,
            Height = request.Height,
            Width = request.Width,
            PanelX = request.PanelX,
            PanelY = request.PanelY,
            RawQuery = request.RawQuery,
        };
    }

    public static UpdatePanelCommand ToUpdateCommand(this UpdatePanelRequest request)
    {
        return new UpdatePanelCommand
        {
            PanelId = request.PanelId,
            Title = request.Title,
            Type = request.Type,
            DataSourceId = request.DataSourceId,
            Height = request.Height,
            Width = request.Width,
            PanelX = request.PanelX,
            PanelY = request.PanelY,
            RawQuery = request.RawQuery,
        };
    }

    public static PanelResponse ToResponse(this PanelEntity panel)
    {
        return new PanelResponse
        {
            PanelId = panel.Id,
            Title = panel.Title,
            Type = panel.PanelType.Name,
            Query = new PanelQueryResponse
            {
                Query = panel.QueryEntity.RawQuery,
                DataSourceName = panel.QueryEntity.DataSourceEntity.Name,
                DataSourceType = panel.QueryEntity.DataSourceEntity.DataSourceType.Name,
                ConnectionString = panel.QueryEntity.DataSourceEntity.ConnectionString
            },
            Height = panel.DataCatLayout.Height,
            Width = panel.DataCatLayout.Width,
            PanelX = panel.DataCatLayout.X,
            PanelY = panel.DataCatLayout.Y,
            DashboardId = panel.ParentDashboardId,
        };
    }
}