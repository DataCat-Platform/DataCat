namespace DataCat.Storage.Postgres.Sql;

public static class DashboardSql
{
     public static class Select
     {
          public const string FindDashboard =
        $"""
         SELECT
              -- Dashboard Details
              d.{Public.Dashboards.Id}                   {nameof(DashboardSnapshot.Id)},
              d.{Public.Dashboards.Name}                 {nameof(DashboardSnapshot.Name)},
              d.{Public.Dashboards.Description}          {nameof(DashboardSnapshot.Description)},
              d.{Public.Dashboards.OwnerId}              {nameof(DashboardSnapshot.OwnerId)},
              d.{Public.Dashboards.CreatedAt}            {nameof(DashboardSnapshot.CreatedAt)},
              d.{Public.Dashboards.UpdatedAt}            {nameof(DashboardSnapshot.UpdatedAt)},
              
              -- Owner Details
              u.{Public.Users.Id}        {nameof(UserSnapshot.UserId)},
              
              -- PanelX Details
              p.{Public.Panels.Id}              {nameof(PanelSnapshot.Id)},
              p.{Public.Panels.Title}           {nameof(PanelSnapshot.Title)},
              p.{Public.Panels.TypeId}          {nameof(PanelSnapshot.TypeId)},
              p.{Public.Panels.RawQuery}        {nameof(PanelSnapshot.RawQuery)},
              p.{Public.Panels.X}               {nameof(PanelSnapshot.X)},
              p.{Public.Panels.Y}               {nameof(PanelSnapshot.Y)},
              p.{Public.Panels.Width}           {nameof(PanelSnapshot.Width)},
              p.{Public.Panels.Height}          {nameof(PanelSnapshot.Height)},
              p.{Public.Panels.DashboardId}     {nameof(PanelSnapshot.DashboardId)},
              
              -- SharedWith 
              sw.{Public.Users.Id}             {nameof(UserSnapshot.UserId)},
         
              -- DataSources
              ds.{Public.DataSources.Id}                   {nameof(DataSourceSnapshot.Id)},
              ds.{Public.DataSources.Name}                 {nameof(DataSourceSnapshot.Name)},
              ds.{Public.DataSources.TypeId}               {nameof(DataSourceSnapshot.TypeId)},
              ds.{Public.DataSources.ConnectionString}     {nameof(DataSourceSnapshot.ConnectionString)}
         FROM
              {Public.DashboardTable} d
         LEFT JOIN {Public.UserTable} u ON d.{Public.Dashboards.OwnerId} = u.{Public.Users.Id}
         LEFT JOIN {Public.PanelTable} p ON d.{Public.Dashboards.Id} = p.{Public.Panels.DashboardId}
         LEFT JOIN {Public.DataSourceTable} ds on p.{Public.Panels.DataSourceId} = ds.{Public.DataSources.Id}
         LEFT JOIN {Public.DashboardUserLinkTable} dul ON d.{Public.Dashboards.Id} = dul.{Public.Dashboards.Id}
         LEFT JOIN {Public.UserTable} sw ON dul.{Public.Users.Id} = sw.{Public.Users.Id}
         WHERE d.{Public.Dashboards.Id} = @p_dashboard_id
         """;
          
     public const string SearchDashboardsTotalCount =
          $"""
           SELECT
                COUNT(*)
           FROM
                {Public.DashboardTable} d
           WHERE d.{Public.Dashboards.Name} ILIKE @p_name
           """;
    
     public const string SearchDashboards =
        $"""
         SELECT
              -- Dashboard Details
              d.{Public.Dashboards.Id}           {nameof(DashboardSnapshot.Id)},
              d.{Public.Dashboards.Name}         {nameof(DashboardSnapshot.Name)},
              d.{Public.Dashboards.Description}  {nameof(DashboardSnapshot.Description)},
              d.{Public.Dashboards.OwnerId}      {nameof(DashboardSnapshot.OwnerId)},
              d.{Public.Dashboards.CreatedAt}    {nameof(DashboardSnapshot.CreatedAt)},
              d.{Public.Dashboards.UpdatedAt}    {nameof(DashboardSnapshot.UpdatedAt)},
              
              -- Owner Details
              u.{Public.Users.Id}                {nameof(UserSnapshot.UserId)},
              
              -- PanelX Details
              p.{Public.Panels.Id}               {nameof(PanelSnapshot.Id)},
              p.{Public.Panels.Title}            {nameof(PanelSnapshot.Title)},
              p.{Public.Panels.TypeId}           {nameof(PanelSnapshot.TypeId)},
              p.{Public.Panels.RawQuery}         {nameof(PanelSnapshot.RawQuery)},
              p.{Public.Panels.X}                {nameof(PanelSnapshot.X)},
              p.{Public.Panels.Y}                {nameof(PanelSnapshot.Y)},
              p.{Public.Panels.Width}            {nameof(PanelSnapshot.Width)},
              p.{Public.Panels.Height}           {nameof(PanelSnapshot.Height)},
               p.{Public.Panels.DashboardId}     {nameof(PanelSnapshot.DashboardId)},
              
              -- SharedWith 
              sw.{Public.Users.Id}               {nameof(UserSnapshot.UserId)},
              
              -- DataSources
              ds.{Public.DataSources.Id}                {nameof(DataSourceSnapshot.Id)},
              ds.{Public.DataSources.Name}              {nameof(DataSourceSnapshot.Name)},
              ds.{Public.DataSources.TypeId}            {nameof(DataSourceSnapshot.TypeId)},
              ds.{Public.DataSources.ConnectionString}  {nameof(DataSourceSnapshot.ConnectionString)}
         FROM
              {Public.DashboardTable} d
         LEFT JOIN {Public.UserTable} u ON d.{Public.Dashboards.OwnerId} = u.{Public.Users.Id}
         LEFT JOIN {Public.PanelTable} p ON d.{Public.Dashboards.Id} = p.{Public.Panels.DashboardId}
         LEFT JOIN {Public.DashboardUserLinkTable} dul ON d.{Public.Dashboards.Id} = dul.{Public.Dashboards.Id}
         LEFT JOIN {Public.UserTable} sw ON dul.{Public.Users.Id} = sw.{Public.Users.Id}
         LEFT JOIN {Public.DataSourceTable} ds on p.{Public.Panels.DataSourceId} = ds.{Public.DataSources.Id}
         WHERE d.{Public.Dashboards.Name} ILIKE @p_name
         LIMIT @limit OFFSET @offset
         """;
     }
}