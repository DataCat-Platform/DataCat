namespace DataCat.Storage.Postgres.Repositories;

public sealed class PanelRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork)
    : IRepository<PanelEntity, Guid>, IPanelRepository
{
    public async Task<PanelEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_panel_id = id.ToString() };

        const string sql = $"""
            SELECT
                {Public.Panels.Id}                   {nameof(PanelSnapshot.Id)},
                {Public.Panels.Title}                {nameof(PanelSnapshot.Title)},
                {Public.Panels.TypeId}               {nameof(PanelSnapshot.TypeId)},
                {Public.Panels.RawQuery}             {nameof(PanelSnapshot.RawQuery)},
                {Public.Panels.DataSourceId}         {nameof(PanelSnapshot.DataSourceId)},
                {Public.Panels.X}                    {nameof(PanelSnapshot.X)},
                {Public.Panels.Y}                    {nameof(PanelSnapshot.Y)},
                {Public.Panels.Width}                {nameof(PanelSnapshot.Width)},
                {Public.Panels.Height}               {nameof(PanelSnapshot.Height)},
                {Public.Panels.DashboardId}          {nameof(PanelSnapshot.DashboardId)},
                
                {Public.DataSources.Id}                 {nameof(DataSourceSnapshot.Id)},
                {Public.DataSources.Name}               {nameof(DataSourceSnapshot.Name)},
                {Public.DataSources.TypeId}             {nameof(DataSourceSnapshot.TypeId)},
                {Public.DataSources.ConnectionString}   {nameof(DataSourceSnapshot.ConnectionString)}
            FROM 
                {Public.PanelTable} p
            LEFT JOIN 
                {Public.DataSourceTable} ds ON ds.{Public.DataSources.Id} = p.{Public.Panels.DataSourceId}
            WHERE {Public.Panels.Id} = @p_panel_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryAsync<PanelSnapshot, DataSourceSnapshot, PanelSnapshot>(
            sql, 
            map: (panel, dataSource) =>
            {
                panel.DataSource = dataSource;
                return panel;
            },
            splitOn: $"{nameof(DataSourceSnapshot.Id)}",
            param: parameters, 
            transaction: UnitOfWork.Transaction);

        return result.FirstOrDefault()?.RestoreFromSnapshot();
    }
    
    public async Task AddAsync(PanelEntity entity, CancellationToken token = default)
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
                {Public.Panels.DashboardId}
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
                @{nameof(PanelSnapshot.DashboardId)}
            )
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task UpdateAsync(PanelEntity entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        const string sql = $"""
            UPDATE {Public.PanelTable}
            SET 
                {Public.Panels.Title}        = @{nameof(PanelSnapshot.Title)},
                {Public.Panels.TypeId}       = @{nameof(PanelSnapshot.TypeId)},
                {Public.Panels.RawQuery}     = @{nameof(PanelSnapshot.RawQuery)},
                {Public.Panels.DataSourceId} = @{nameof(PanelSnapshot.DataSourceId)},
                {Public.Panels.X}            = @{nameof(PanelSnapshot.X)},
                {Public.Panels.Y}            = @{nameof(PanelSnapshot.Y)},
                {Public.Panels.Width}        = @{nameof(PanelSnapshot.Width)},
                {Public.Panels.Height}       = @{nameof(PanelSnapshot.Height)}
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