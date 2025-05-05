namespace DataCat.Storage.Postgres.Repositories;

public sealed class VariableRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork)
    : IRepository<Variable, Guid>, IVariableRepository
{
    public async Task<Variable?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_variable_id = id.ToString() };
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = $"""
            SELECT
                {Public.Variables.Id}              AS {nameof(VariableSnapshot.Id)},
                {Public.Variables.Placeholder}     AS {nameof(VariableSnapshot.Placeholder)},
                {Public.Variables.Value}           AS {nameof(VariableSnapshot.Value)},
                {Public.Variables.NamespaceId}     AS {nameof(VariableSnapshot.NamespaceId)},
                {Public.Variables.DashboardId}     AS {nameof(VariableSnapshot.DashboardId)}
            FROM {Public.VariableTable}
            WHERE {Public.Variables.Id} = @p_variable_id
        """;

        var result = await connection.QueryAsync<VariableSnapshot>(
            sql, 
            parameters, 
            transaction: UnitOfWork.Transaction);
        
        var snapshot = result.FirstOrDefault();
        return snapshot?.RestoreFromSnapshot();
    }

    public async Task AddAsync(Variable entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();
        const string sql = $"""
            INSERT INTO {Public.VariableTable} (
                {Public.Variables.Id},
                {Public.Variables.Placeholder},
                {Public.Variables.Value},
                {Public.Variables.NamespaceId},
                {Public.Variables.DashboardId})
            VALUES (
                @{nameof(VariableSnapshot.Id)},
                @{nameof(VariableSnapshot.Placeholder)},
                @{nameof(VariableSnapshot.Value)},
                @{nameof(VariableSnapshot.NamespaceId)},
                @{nameof(VariableSnapshot.DashboardId)})
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task<List<Variable>> GetAllAsyncForDashboardAsync(Guid dashboardId, CancellationToken token = default)
    {
        var parameters = new { p_dashboard_id = dashboardId.ToString() };
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = $"""
            SELECT
                {Public.Variables.Id}              AS {nameof(VariableSnapshot.Id)},
                {Public.Variables.Placeholder}     AS {nameof(VariableSnapshot.Placeholder)},
                {Public.Variables.Value}           AS {nameof(VariableSnapshot.Value)},
                {Public.Variables.NamespaceId}     AS {nameof(VariableSnapshot.NamespaceId)},
                {Public.Variables.DashboardId}     AS {nameof(VariableSnapshot.DashboardId)}
            FROM {Public.VariableTable}
            WHERE {Public.Variables.DashboardId} = @p_dashboard_id
        """;

        var result = await connection.QueryAsync<VariableSnapshot>(
            sql, 
            parameters, 
            transaction: UnitOfWork.Transaction);
        
        return result.Select(x => x.RestoreFromSnapshot()).ToList();
    }

    public async Task<List<Variable>> GetAllAsyncForNamespaceAsync(Guid namespaceId, CancellationToken token = default)
    {
        var parameters = new { p_namespace_id = namespaceId.ToString() };
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = $"""
            SELECT
                {Public.Variables.Id}              AS {nameof(VariableSnapshot.Id)},
                {Public.Variables.Placeholder}     AS {nameof(VariableSnapshot.Placeholder)},
                {Public.Variables.Value}           AS {nameof(VariableSnapshot.Value)},
                {Public.Variables.NamespaceId}     AS {nameof(VariableSnapshot.NamespaceId)},
                {Public.Variables.DashboardId}     AS {nameof(VariableSnapshot.DashboardId)}
            FROM {Public.VariableTable}
            WHERE {Public.Variables.NamespaceId} = @p_namespace_id
        """;

        var result = await connection.QueryAsync<VariableSnapshot>(
            sql, 
            parameters,
            transaction: UnitOfWork.Transaction);
        
        return result.Select(x => x.RestoreFromSnapshot()).ToList();
    }

    public async Task UpdateAsync(Variable variable, CancellationToken token = default)
    {
        var snapshot = variable.Save();
        const string sql = $"""
            UPDATE {Public.VariableTable} SET
                {Public.Variables.Placeholder} = @{nameof(VariableSnapshot.Placeholder)},
                {Public.Variables.Value}       = @{nameof(VariableSnapshot.Value)},
                {Public.Variables.NamespaceId} = @{nameof(VariableSnapshot.NamespaceId)},
                {Public.Variables.DashboardId} = @{nameof(VariableSnapshot.DashboardId)}
            WHERE {Public.Variables.Id} = @{nameof(VariableSnapshot.Id)}
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_variable_id = id.ToString() };
        const string sql = $"""
            DELETE FROM {Public.VariableTable}
            WHERE {Public.Variables.Id} = @p_variable_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: UnitOfWork.Transaction);
    }
}