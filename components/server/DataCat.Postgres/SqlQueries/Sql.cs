namespace DataCat.Server.Postgres.SqlQueries;

public static class Sql
{
    public static readonly string FindDashboard =
        $"""
         SELECT
              -- Dashboard Details
              d.{Public.Dashboards.DashboardId},
              d.{Public.Dashboards.DashboardName},
              d.{Public.Dashboards.DashboardDescription},
              d.{Public.Dashboards.DashboardOwnerId},
              d.{Public.Dashboards.DashboardCreatedAt},
              d.{Public.Dashboards.DashboardUpdatedAt},
              
              -- Owner Details
              u.{Public.Users.UserId},
              u.{Public.Users.UserName},
              u.{Public.Users.UserEmail},
              u.{Public.Users.UserRole},
              
              -- PanelX Details
              p.{Public.Panels.PanelId},
              p.{Public.Panels.PanelTitle},
              p.{Public.Panels.PanelType},
              p.{Public.Panels.PanelRawQuery},
              p.{Public.Panels.PanelX},
              p.{Public.Panels.PanelY},
              p.{Public.Panels.PanelWidth},
              p.{Public.Panels.PanelHeight},
              p.{Public.Panels.PanelParentDashboardId},
              
              -- SharedWith 
              sw.{Public.Users.UserId},
              sw.{Public.Users.UserName},
              sw.{Public.Users.UserEmail},
              sw.{Public.Users.UserRole},
         
              -- DataSources
              ds.{Public.DataSources.DataSourceId},
              ds.{Public.DataSources.DataSourceName},
              ds.{Public.DataSources.DataSourceType},
              ds.{Public.DataSources.DataSourceConnectionString}
         FROM
              {Public.DashboardTable} d
         LEFT JOIN {Public.UserTable} u ON d.{Public.Dashboards.DashboardOwnerId} = u.{Public.Users.UserId}
         LEFT JOIN {Public.PanelTable} p ON d.{Public.Dashboards.DashboardId} = p.{Public.Panels.PanelParentDashboardId}
         LEFT JOIN {Public.DataSourceTable} ds on p.{Public.Panels.PanelDataSourceId} = ds.{Public.DataSources.DataSourceId}
         LEFT JOIN {Public.DashboardUserLinkTable} dul ON d.{Public.Dashboards.DashboardId} = dul.{Public.Dashboards.DashboardId}
         LEFT JOIN {Public.UserTable} sw ON dul.{Public.Users.UserId} = sw.{Public.Users.UserId}
         WHERE d.{Public.Dashboards.DashboardId} = @DashboardId
         """;
    
    public static readonly string SearchDashboards =
        $"""
         SELECT
              -- Dashboard Details
              d.{Public.Dashboards.DashboardId},
              d.{Public.Dashboards.DashboardName},
              d.{Public.Dashboards.DashboardDescription},
              d.{Public.Dashboards.DashboardOwnerId},
              d.{Public.Dashboards.DashboardCreatedAt},
              d.{Public.Dashboards.DashboardUpdatedAt},
              
              -- Owner Details
              u.{Public.Users.UserId},
              u.{Public.Users.UserName},
              u.{Public.Users.UserEmail},
              u.{Public.Users.UserRole},
              
              -- PanelX Details
              p.{Public.Panels.PanelId},
              p.{Public.Panels.PanelTitle},
              p.{Public.Panels.PanelType},
              p.{Public.Panels.PanelRawQuery},
              p.{Public.Panels.PanelX},
              p.{Public.Panels.PanelY},
              p.{Public.Panels.PanelWidth},
              p.{Public.Panels.PanelHeight},
              p.{Public.Panels.PanelParentDashboardId},
              
              -- SharedWith 
              sw.{Public.Users.UserId},
              sw.{Public.Users.UserName},
              sw.{Public.Users.UserEmail},
              sw.{Public.Users.UserRole}
         FROM
              {Public.DashboardTable} d
         LEFT JOIN {Public.UserTable} u ON d.{Public.Dashboards.DashboardOwnerId} = u.{Public.Users.UserId}
         LEFT JOIN {Public.PanelTable} p ON d.{Public.Dashboards.DashboardId} = p.{Public.Panels.PanelParentDashboardId}
         LEFT JOIN {Public.DashboardUserLinkTable} dul ON d.{Public.Dashboards.DashboardId} = dul.{Public.Dashboards.DashboardId}
         LEFT JOIN {Public.UserTable} sw ON dul.{Public.Users.UserId} = sw.{Public.Users.UserId}
         WHERE d.{Public.Dashboards.DashboardName} ILIKE @Name
         LIMIT @PageSize OFFSET @Page
         """;
}