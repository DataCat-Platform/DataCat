namespace DataCat.Storage.Postgres.Repositories;

public sealed class PanelRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork,
    NamespaceContext NamespaceContext)
    : IRepository<Panel, Guid>, IPanelRepository
{
    public async Task<Panel?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_panel_id = id.ToString(), p_namespace_id = NamespaceContext.NamespaceId };

        const string sql = $"""
            SELECT
                p.{Public.Panels.Id}                         {nameof(PanelSnapshot.Id)},
                p.{Public.Panels.Title}                      {nameof(PanelSnapshot.Title)},
                p.{Public.Panels.TypeId}                     {nameof(PanelSnapshot.TypeId)},
                p.{Public.Panels.RawQuery}                   {nameof(PanelSnapshot.RawQuery)},
                p.{Public.Panels.DataSourceId}               {nameof(PanelSnapshot.DataSourceId)},
                p.{Public.Panels.LayoutConfiguration}        {nameof(PanelSnapshot.LayoutConfiguration)},
                p.{Public.Panels.DashboardId}                {nameof(PanelSnapshot.DashboardId)},
                p.{Public.Panels.StylingConfiguration}       {nameof(PanelSnapshot.StyleConfiguration)},
                p.{Public.Panels.NamespaceId}                {nameof(PanelSnapshot.NamespaceId)},
                
                ds.{Public.DataSources.Id}                   {nameof(DataSourceSnapshot.Id)},
                ds.{Public.DataSources.Name}                 {nameof(DataSourceSnapshot.Name)},
                ds.{Public.DataSources.TypeId}               {nameof(DataSourceSnapshot.TypeId)},
                ds.{Public.DataSources.ConnectionSettings}   {nameof(DataSourceSnapshot.ConnectionSettings)},
                ds.{Public.DataSources.Purpose}              {nameof(DataSourceSnapshot.Purpose)},
                
                dst.{Public.DataSourceType.Id}              {nameof(DataSourceTypeSnapshot.Id)},
                dst.{Public.DataSourceType.Name}            {nameof(DataSourceTypeSnapshot.Name)}
        
            FROM 
                {Public.PanelTable} p
            JOIN 
                {Public.DataSourceTable} ds ON ds.{Public.DataSources.Id} = p.{Public.Panels.DataSourceId}
            JOIN 
                {Public.DataSourceTypeTable} dst ON dst.{Public.DataSourceType.Id} = ds.{Public.DataSources.TypeId} 
            WHERE p.{Public.Panels.Id} = @p_panel_id AND p.{Public.Panels.NamespaceId} = @p_namespace_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryAsync<PanelSnapshot, DataSourceSnapshot, DataSourceTypeSnapshot, PanelSnapshot>(
            sql, 
            map: (panel, dataSource, dataSourceTypeSnapshot) =>
            {
                dataSource.DataSourceType = dataSourceTypeSnapshot;
                panel.DataSource = dataSource;
                return panel;
            },
            splitOn: $"{nameof(DataSourceSnapshot.Id)}, {nameof(DataSourceTypeSnapshot.Id)}",
            param: parameters, 
            transaction: UnitOfWork.Transaction);

        return result.FirstOrDefault()?.RestoreFromSnapshot();
    }
    
    public async Task AddAsync(Panel entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        const string sql = $"""
            INSERT INTO {Public.PanelTable} (
                {Public.Panels.Id},
                {Public.Panels.Title},
                {Public.Panels.TypeId},
                {Public.Panels.RawQuery},
                {Public.Panels.DataSourceId},
                {Public.Panels.LayoutConfiguration},
                {Public.Panels.DashboardId},
                {Public.Panels.StylingConfiguration},
                {Public.Panels.NamespaceId}
            )
            VALUES (
                @{nameof(PanelSnapshot.Id)},
                @{nameof(PanelSnapshot.Title)},
                @{nameof(PanelSnapshot.TypeId)},
                @{nameof(PanelSnapshot.RawQuery)},
                @{nameof(PanelSnapshot.DataSourceId)},
                @{nameof(PanelSnapshot.LayoutConfiguration)},
                @{nameof(PanelSnapshot.DashboardId)},
                @{nameof(PanelSnapshot.StyleConfiguration)},
                @{nameof(PanelSnapshot.NamespaceId)}
            )
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task UpdateAsync(Panel entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        const string sql = $"""
            UPDATE {Public.PanelTable}
            SET 
                {Public.Panels.Title}                      = @{nameof(PanelSnapshot.Title)},
                {Public.Panels.TypeId}                     = @{nameof(PanelSnapshot.TypeId)},
                {Public.Panels.RawQuery}                   = @{nameof(PanelSnapshot.RawQuery)},
                {Public.Panels.DataSourceId}               = @{nameof(PanelSnapshot.DataSourceId)},
                {Public.Panels.LayoutConfiguration}        = @{nameof(PanelSnapshot.LayoutConfiguration)},
                {Public.Panels.StylingConfiguration}       = @{nameof(PanelSnapshot.StyleConfiguration)}
            WHERE {Public.Panels.Id} = @{nameof(PanelSnapshot.Id)}
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_panel_id = id.ToString() };

        const string sql = $"""
            DELETE FROM {Public.PanelTable}
            WHERE {Public.Panels.Id} = @p_panel_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: UnitOfWork.Transaction);
    }
}