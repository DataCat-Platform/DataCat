namespace DataCat.Server.Postgres.SqlQueries;

public static class MapFunctions
{
    public static Func<DashboardSnapshot, UserSnapshot, PanelSnapshot?, UserSnapshot?, DashboardSnapshot> MapDashboard(
        Dictionary<string, DashboardSnapshot> dashboardDictionary)
        => (dashboard, userOwner, panel, sharedUser) =>
        {
            if (!dashboardDictionary.TryGetValue(dashboard.DashboardId, out var existingDashboard))
            {
                dashboard.Owner = userOwner;
                dashboard.Panels = new List<PanelSnapshot>();
                dashboard.SharedWith = new List<UserSnapshot>();
                dashboardDictionary.Add(dashboard.DashboardId, dashboard);
                existingDashboard = dashboard;
            }

            if (panel is not null && existingDashboard.Panels.All(p => p.PanelId != panel.PanelId))
            {
                existingDashboard.Panels.Add(panel);
            }

            if (sharedUser is not null && existingDashboard.SharedWith.All(u => u.UserId != sharedUser.UserId))
            {
                existingDashboard.SharedWith.Add(sharedUser);
            }

            return existingDashboard;
        };

}