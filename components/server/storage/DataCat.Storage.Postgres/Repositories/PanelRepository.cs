namespace DataCat.Storage.Postgres.Repositories;

public sealed class PanelRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork)
    : IRepository<Panel, Guid>, IPanelRepository
{
    public async Task<Panel?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_panel_id = id.ToString() };

        const string sql = $"""
            SELECT
                p.{Public.Panels.Id}                         {nameof(PanelSnapshot.Id)},
                p.{Public.Panels.Title}                      {nameof(PanelSnapshot.Title)},
                p.{Public.Panels.TypeId}                     {nameof(PanelSnapshot.TypeId)},
                p.{Public.Panels.RawQuery}                   {nameof(PanelSnapshot.RawQuery)},
                p.{Public.Panels.DataSourceId}               {nameof(PanelSnapshot.DataSourceId)},
                p.{Public.Panels.X}                          {nameof(PanelSnapshot.X)},
                p.{Public.Panels.Y}                          {nameof(PanelSnapshot.Y)},
                p.{Public.Panels.Width}                      {nameof(PanelSnapshot.Width)},
                p.{Public.Panels.Height}                     {nameof(PanelSnapshot.Height)},
                p.{Public.Panels.DashboardId}                {nameof(PanelSnapshot.DashboardId)},
                p.{Public.Panels.StylingConfiguration}       {nameof(PanelSnapshot.StyleConfiguration)},
                
                ds.{Public.DataSources.Id}                   {nameof(DataSourceSnapshot.Id)},
                ds.{Public.DataSources.Name}                 {nameof(DataSourceSnapshot.Name)},
                ds.{Public.DataSources.TypeId}               {nameof(DataSourceSnapshot.TypeId)},
                ds.{Public.DataSources.ConnectionSettings}   {nameof(DataSourceSnapshot.ConnectionSettings)},
                
                dst.{Public.DataSourceType.Id}              {nameof(DataSourceTypeSnapshot.Id)},
                dst.{Public.DataSourceType.Name}            {nameof(DataSourceTypeSnapshot.Name)}
        
            FROM 
                {Public.PanelTable} p
            JOIN 
                {Public.DataSourceTable} ds ON ds.{Public.DataSources.Id} = p.{Public.Panels.DataSourceId}
            JOIN 
                {Public.DataSourceTypeTable} dst ON dst.{Public.DataSourceType.Id} = ds.{Public.DataSources.TypeId} 
            WHERE {Public.Panels.Id} = @p_panel_id
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
                {Public.Panels.X},
                {Public.Panels.Y},
                {Public.Panels.Width},
                {Public.Panels.Height},
                {Public.Panels.DashboardId},
                {Public.Panels.StylingConfiguration}
            )
            VALUES (
                @{nameof(PanelSnapshot.Id)},
                @{nameof(PanelSnapshot.TypeId)},
                @{nameof(PanelSnapshot.TypeId)},
                @{nameof(PanelSnapshot.RawQuery)},
                @{nameof(PanelSnapshot.DataSourceId)},
                @{nameof(PanelSnapshot.X)},
                @{nameof(PanelSnapshot.Y)},
                @{nameof(PanelSnapshot.Width)},
                @{nameof(PanelSnapshot.Height)},
                @{nameof(PanelSnapshot.DashboardId)},
                @{nameof(PanelSnapshot.StyleConfiguration)}
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
                {Public.Panels.X}                          = @{nameof(PanelSnapshot.X)},
                {Public.Panels.Y}                          = @{nameof(PanelSnapshot.Y)},
                {Public.Panels.Width}                      = @{nameof(PanelSnapshot.Width)},
                {Public.Panels.Height}                     = @{nameof(PanelSnapshot.Height)},
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