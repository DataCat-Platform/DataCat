namespace DataCat.Storage.Postgres.Sql;

public static class DashboardSql
{
     public static class Select
     {
          public const string FindDashboard =
        $"""
         SELECT
              -- Dashboard Details
              dashboard.{Public.Dashboards.Id}                   {nameof(DashboardSnapshot.Id)},
              dashboard.{Public.Dashboards.Name}                 {nameof(DashboardSnapshot.Name)},
              dashboard.{Public.Dashboards.Description}          {nameof(DashboardSnapshot.Description)},
              dashboard.{Public.Dashboards.OwnerId}              {nameof(DashboardSnapshot.OwnerId)},
              dashboard.{Public.Dashboards.NamespaceId}          {nameof(DashboardSnapshot.NamespaceId)},
              dashboard.{Public.Dashboards.CreatedAt}            {nameof(DashboardSnapshot.CreatedAt)},
              dashboard.{Public.Dashboards.UpdatedAt}            {nameof(DashboardSnapshot.UpdatedAt)},
              dashboard.{Public.Dashboards.Tags}                 {nameof(DashboardSnapshot.Tags)},
              
              -- Owner Details
              owner.{Public.Users.Id}        {nameof(UserSnapshot.UserId)},
              
              -- PanelX Details
              panel.{Public.Panels.Id}                       {nameof(PanelSnapshot.Id)},
              panel.{Public.Panels.Title}                    {nameof(PanelSnapshot.Title)},
              panel.{Public.Panels.TypeId}                   {nameof(PanelSnapshot.TypeId)},
              panel.{Public.Panels.RawQuery}                 {nameof(PanelSnapshot.RawQuery)},
              panel.{Public.Panels.X}                        {nameof(PanelSnapshot.X)},
              panel.{Public.Panels.Y}                        {nameof(PanelSnapshot.Y)},
              panel.{Public.Panels.Width}                    {nameof(PanelSnapshot.Width)},
              panel.{Public.Panels.Height}                   {nameof(PanelSnapshot.Height)},
              panel.{Public.Panels.DashboardId}              {nameof(PanelSnapshot.DashboardId)},
              panel.{Public.Panels.StylingConfiguration}     {nameof(PanelSnapshot.StyleConfiguration)},
              
              -- SharedWith 
              shared_with.{Public.Users.Id}             {nameof(UserSnapshot.UserId)},
         
              -- DataSources
              data_source.{Public.DataSources.Id}                     {nameof(DataSourceSnapshot.Id)},
              data_source.{Public.DataSources.Name}                   {nameof(DataSourceSnapshot.Name)},
              data_source.{Public.DataSources.TypeId}                 {nameof(DataSourceSnapshot.TypeId)},
              data_source.{Public.DataSources.ConnectionSettings}     {nameof(DataSourceSnapshot.ConnectionSettings)},
              data_source.{Public.DataSources.Purpose}                {nameof(DataSourceSnapshot.Purpose)},
              
              data_source_type.{Public.DataSourceType.Id}     {nameof(DataSourceTypeSnapshot.Id)},
              data_source_type.{Public.DataSourceType.Name}   {nameof(DataSourceTypeSnapshot.Name)}
              
         FROM
              {Public.DashboardTable} dashboard
         JOIN {Public.UserTable} owner ON dashboard.{Public.Dashboards.OwnerId} = owner.{Public.Users.Id}
         JOIN {Public.PanelTable} panel ON dashboard.{Public.Dashboards.Id} = panel.{Public.Panels.DashboardId}
         JOIN {Public.DataSourceTable} data_source on panel.{Public.Panels.DataSourceId} = data_source.{Public.DataSources.Id}
         JOIN {Public.DataSourceTypeTable} data_source_type ON data_source_type.{Public.DataSourceType.Id} = data_source.{Public.DataSources.TypeId} 
         JOIN {Public.DashboardUserLinkTable} dul ON dashboard.{Public.Dashboards.Id} = dul.{Public.Dashboards.Id}
         JOIN {Public.UserTable} shared_with ON dul.{Public.Users.Id} = shared_with.{Public.Users.Id}
         WHERE dashboard.{Public.Dashboards.Id} = @p_dashboard_id
         """;
          
     public const string SearchDashboardsTotalCount =
          $"""
           SELECT
                COUNT(*)
           FROM
                {Public.DashboardTable} d
           WHERE 1=1 
           """;
    
     public const string SearchDashboards =
        $"""
         SELECT
              -- Dashboard Details
              dashboard.{Public.Dashboards.Id}                   {nameof(DashboardSnapshot.Id)},
              dashboard.{Public.Dashboards.Name}                 {nameof(DashboardSnapshot.Name)},
              dashboard.{Public.Dashboards.Description}          {nameof(DashboardSnapshot.Description)},
              dashboard.{Public.Dashboards.OwnerId}              {nameof(DashboardSnapshot.OwnerId)},
              dashboard.{Public.Dashboards.NamespaceId}          {nameof(DashboardSnapshot.NamespaceId)},
              dashboard.{Public.Dashboards.CreatedAt}            {nameof(DashboardSnapshot.CreatedAt)},
              dashboard.{Public.Dashboards.UpdatedAt}            {nameof(DashboardSnapshot.UpdatedAt)},
              dashboard.{Public.Dashboards.Tags}                 {nameof(DashboardSnapshot.Tags)},
              
              -- Owner Details
              owner.{Public.Users.Id}                            {nameof(UserSnapshot.UserId)},
              
              -- PanelX Details
              panel.{Public.Panels.Id}                           {nameof(PanelSnapshot.Id)},
              panel.{Public.Panels.Title}                        {nameof(PanelSnapshot.Title)},
              panel.{Public.Panels.TypeId}                       {nameof(PanelSnapshot.TypeId)},
              panel.{Public.Panels.RawQuery}                     {nameof(PanelSnapshot.RawQuery)},
              panel.{Public.Panels.X}                            {nameof(PanelSnapshot.X)},
              panel.{Public.Panels.Y}                            {nameof(PanelSnapshot.Y)},
              panel.{Public.Panels.Width}                        {nameof(PanelSnapshot.Width)},
              panel.{Public.Panels.Height}                       {nameof(PanelSnapshot.Height)},
              panel.{Public.Panels.DashboardId}                  {nameof(PanelSnapshot.DashboardId)},
              panel.{Public.Panels.StylingConfiguration}         {nameof(PanelSnapshot.StyleConfiguration)},
              
              -- SharedWith 
              shared_with.{Public.Users.Id}                      {nameof(UserSnapshot.UserId)},
              
              -- DataSources
              data_source.{Public.DataSources.Id}                   {nameof(DataSourceSnapshot.Id)},
              data_source.{Public.DataSources.Name}                 {nameof(DataSourceSnapshot.Name)},
              data_source.{Public.DataSources.TypeId}               {nameof(DataSourceSnapshot.TypeId)},
              data_source.{Public.DataSources.ConnectionSettings}   {nameof(DataSourceSnapshot.ConnectionSettings)},
              data_source.{Public.DataSources.Purpose}              {nameof(DataSourceSnapshot.Purpose)},
              
              data_source_type.{Public.DataSourceType.Id}          {nameof(DataSourceTypeSnapshot.Id)},
              data_source_type.{Public.DataSourceType.Name}        {nameof(DataSourceTypeSnapshot.Name)}
         
         FROM
              {Public.DashboardTable} dashboard
         JOIN {Public.UserTable} owner ON dashboard.{Public.Dashboards.OwnerId} = owner.{Public.Users.Id}
         JOIN {Public.PanelTable} panel ON dashboard.{Public.Dashboards.Id} = panel.{Public.Panels.DashboardId}
         JOIN {Public.DataSourceTable} data_source on panel.{Public.Panels.DataSourceId} = data_source.{Public.DataSources.Id}
         JOIN {Public.DataSourceTypeTable} data_source_type ON data_source_type.{Public.DataSourceType.Id} = data_source.{Public.DataSources.TypeId}
         JOIN {Public.DashboardUserLinkTable} dul ON dashboard.{Public.Dashboards.Id} = dul.{Public.Dashboards.Id}
         JOIN {Public.UserTable} shared_with ON dul.{Public.Users.Id} = shared_with.{Public.Users.Id}
         WHERE 1=1 
         """;
     
     public const string GetDashboardsByNamespaceId =
        $"""
         SELECT
              -- Dashboard Details
              dashboards.{Public.Dashboards.Id}                       {nameof(DashboardSnapshot.Id)},
              dashboards.{Public.Dashboards.Name}                     {nameof(DashboardSnapshot.Name)},
              dashboards.{Public.Dashboards.Description}              {nameof(DashboardSnapshot.Description)},
              dashboards.{Public.Dashboards.OwnerId}                  {nameof(DashboardSnapshot.OwnerId)},
              dashboards.{Public.Dashboards.NamespaceId}              {nameof(DashboardSnapshot.NamespaceId)},
              dashboards.{Public.Dashboards.CreatedAt}                {nameof(DashboardSnapshot.CreatedAt)},
              dashboards.{Public.Dashboards.UpdatedAt}                {nameof(DashboardSnapshot.UpdatedAt)},
              dashboards.{Public.Dashboards.Tags}                     {nameof(DashboardSnapshot.Tags)}
         FROM
              {Public.DashboardTable} dashboards
         WHERE dashboards.{Public.Dashboards.NamespaceId} = @p_namespace_id
         """;
     }
}