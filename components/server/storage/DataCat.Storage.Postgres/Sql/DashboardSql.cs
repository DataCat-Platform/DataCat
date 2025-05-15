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
              dashboard.{Public.Dashboards.NamespaceId}          {nameof(DashboardSnapshot.NamespaceId)},
              dashboard.{Public.Dashboards.CreatedAt}            {nameof(DashboardSnapshot.CreatedAt)},
              dashboard.{Public.Dashboards.UpdatedAt}            {nameof(DashboardSnapshot.UpdatedAt)},
              dashboard.{Public.Dashboards.Tags}                 {nameof(DashboardSnapshot.Tags)},
              
              -- PanelX Details
              panel.{Public.Panels.Id}                       {nameof(PanelSnapshot.Id)},
              panel.{Public.Panels.Title}                    {nameof(PanelSnapshot.Title)},
              panel.{Public.Panels.TypeId}                   {nameof(PanelSnapshot.TypeId)},
              panel.{Public.Panels.RawQuery}                 {nameof(PanelSnapshot.RawQuery)},
              panel.{Public.Panels.LayoutConfiguration}      {nameof(PanelSnapshot.LayoutConfiguration)},
              panel.{Public.Panels.DashboardId}              {nameof(PanelSnapshot.DashboardId)},
              panel.{Public.Panels.StylingConfiguration}     {nameof(PanelSnapshot.StyleConfiguration)},
              panel.{Public.Panels.NamespaceId}              {nameof(PanelSnapshot.NamespaceId)},
              
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
         LEFT JOIN {Public.PanelTable} panel ON dashboard.{Public.Dashboards.Id} = panel.{Public.Panels.DashboardId}
         LEFT JOIN {Public.DataSourceTable} data_source on panel.{Public.Panels.DataSourceId} = data_source.{Public.DataSources.Id}
         LEFT JOIN {Public.DataSourceTypeTable} data_source_type ON data_source_type.{Public.DataSourceType.Id} = data_source.{Public.DataSources.TypeId} 
         WHERE dashboard.{Public.Dashboards.Id} = @p_dashboard_id AND dashboard.{Public.Dashboards.NamespaceId} = @p_namespace_id
         """;
          
     public const string SearchDashboardsTotalCount =
          $"""
           SELECT
                COUNT(*)
           FROM
                {Public.DashboardTable} dashboard
           WHERE dashboard.{Public.Dashboards.NamespaceId} = @p_namespace_id 
           """;
    
     public const string SearchDashboards =
        $"""
         SELECT
              -- Dashboard Details
              dashboard.{Public.Dashboards.Id}                   {nameof(DashboardSnapshot.Id)},
              dashboard.{Public.Dashboards.Name}                 {nameof(DashboardSnapshot.Name)},
              dashboard.{Public.Dashboards.Description}          {nameof(DashboardSnapshot.Description)},
              dashboard.{Public.Dashboards.NamespaceId}          {nameof(DashboardSnapshot.NamespaceId)},
              dashboard.{Public.Dashboards.CreatedAt}            {nameof(DashboardSnapshot.CreatedAt)},
              dashboard.{Public.Dashboards.UpdatedAt}            {nameof(DashboardSnapshot.UpdatedAt)},
              dashboard.{Public.Dashboards.Tags}                 {nameof(DashboardSnapshot.Tags)},
              
              -- PanelX Details
              panel.{Public.Panels.Id}                           {nameof(PanelSnapshot.Id)},
              panel.{Public.Panels.Title}                        {nameof(PanelSnapshot.Title)},
              panel.{Public.Panels.TypeId}                       {nameof(PanelSnapshot.TypeId)},
              panel.{Public.Panels.RawQuery}                     {nameof(PanelSnapshot.RawQuery)},
              panel.{Public.Panels.LayoutConfiguration}          {nameof(PanelSnapshot.LayoutConfiguration)},
              panel.{Public.Panels.DashboardId}                  {nameof(PanelSnapshot.DashboardId)},
              panel.{Public.Panels.StylingConfiguration}         {nameof(PanelSnapshot.StyleConfiguration)},
              panel.{Public.Panels.NamespaceId}                  {nameof(PanelSnapshot.NamespaceId)},
              
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
         LEFT JOIN {Public.PanelTable} panel ON dashboard.{Public.Dashboards.Id} = panel.{Public.Panels.DashboardId}
         LEFT JOIN {Public.DataSourceTable} data_source on panel.{Public.Panels.DataSourceId} = data_source.{Public.DataSources.Id}
         LEFT JOIN {Public.DataSourceTypeTable} data_source_type ON data_source_type.{Public.DataSourceType.Id} = data_source.{Public.DataSources.TypeId}
         WHERE dashboard.{Public.Dashboards.NamespaceId} = @p_namespace_id 
         """;
     
     public const string GetDashboardsByNamespaceId =
        $"""
         SELECT
              -- Dashboard Details
              dashboards.{Public.Dashboards.Id}                       {nameof(DashboardSnapshot.Id)},
              dashboards.{Public.Dashboards.NamespaceId}              {nameof(DashboardSnapshot.NamespaceId)},
              dashboards.{Public.Dashboards.Name}                     {nameof(DashboardSnapshot.Name)},
              dashboards.{Public.Dashboards.Description}              {nameof(DashboardSnapshot.Description)},
              dashboards.{Public.Dashboards.CreatedAt}                {nameof(DashboardSnapshot.CreatedAt)},
              dashboards.{Public.Dashboards.UpdatedAt}                {nameof(DashboardSnapshot.UpdatedAt)},
              dashboards.{Public.Dashboards.Tags}                     {nameof(DashboardSnapshot.Tags)}
         FROM
              {Public.DashboardTable} dashboards
         WHERE dashboards.{Public.Dashboards.NamespaceId} = @p_namespace_id
         """;
     }
}