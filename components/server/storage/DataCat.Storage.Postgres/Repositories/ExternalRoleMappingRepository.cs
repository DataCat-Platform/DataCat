namespace DataCat.Storage.Postgres.Repositories;

public sealed class ExternalRoleMappingRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork) : IExternalRoleMappingRepository
{
    public async Task AddExternalRoleMappingAsync(ExternalRoleMappingValue externalRoleMappingValue, CancellationToken token = default)
    {
        var parameters = new
        {
            p_external_role = externalRoleMappingValue.ExternalRole,
            p_namespace_id = externalRoleMappingValue.NamespaceId.ToString(),
            p_internal_role_id = externalRoleMappingValue.Role.Value
        };

        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = $"""
            INSERT INTO {Public.ExternalRoleMappingTable} (
                {Public.ExternalRoleMappings.ExternalRole},
                {Public.ExternalRoleMappings.NamespaceId},
                {Public.ExternalRoleMappings.InternalRoleId}
            )
            VALUES (
                @p_external_role,
                @p_namespace_id,
                @p_internal_role_id
            )
            ON CONFLICT DO NOTHING
        """;

        await connection.ExecuteAsync(sql, parameters, unitOfWork.Transaction);
    }

    public async Task<List<ExternalRoleMappingValue>> GetExternalRoleMappingsByExternalRolesAsync(string[] externalRoles, CancellationToken token = default)
    {
        var parameters = new
        {
            p_external_roles = externalRoles
        };

        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = $"""
            SELECT 
                {Public.ExternalRoleMappings.ExternalRole}    {nameof(ExternalRoleMappingSnapshot.ExternalRole)},
                {Public.ExternalRoleMappings.NamespaceId}     {nameof(ExternalRoleMappingSnapshot.NamespaceId)},
                {Public.ExternalRoleMappings.InternalRoleId}  {nameof(ExternalRoleMappingSnapshot.RoleId)}
            FROM {Public.ExternalRoleMappingTable}
            WHERE {Public.ExternalRoleMappings.ExternalRole} = ANY(@p_external_roles)
        """;

        var result = await connection.QueryAsync<ExternalRoleMappingSnapshot>(
            sql,
            param: parameters,
            transaction: unitOfWork.Transaction
        );

        return result.Select(x => x.RestoreFromSnapshot()).ToList();
    }
}