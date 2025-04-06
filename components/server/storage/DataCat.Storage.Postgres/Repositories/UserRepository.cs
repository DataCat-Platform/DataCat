
namespace DataCat.Storage.Postgres.Repositories;

public sealed class UserRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork)
    : IRepository<UserEntity, Guid>, IUserRepository
{
    public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_user_id = id.ToString() };
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = UserSql.Select.FindById;
        
        var userDict = new Dictionary<string, UserSnapshot>();
        
        var result = await connection.QueryAsync<UserSnapshot, AssignedUserRoleSnapshot?, AssignedUserPermissionSnapshot?, UserSnapshot>(
            sql,
            map: (user, role, permission) =>
            {
                if (!userDict.TryGetValue(user.UserId, out _))
                {
                    user.Roles = [];
                    user.Permissions = [];
                    userDict[user.UserId] = user;
                }

                if (role is not null 
                    && !userDict[user.UserId].Roles.Any(
                        r => r.RoleId == role.RoleId && r.NamespaceId == role.NamespaceId))
                {
                    userDict[user.UserId].Roles.Add(role);
                }

                if (permission is not null 
                    && !userDict[user.UserId].Permissions.Any(p =>
                        p.PermissionId == permission.PermissionId && p.NamespaceId == permission.NamespaceId))
                {
                    userDict[user.UserId].Permissions.Add(permission);
                }

                return user;
            }, 
            splitOn: $"{nameof(AssignedUserRoleSnapshot.RoleId)}, {nameof(AssignedUserPermissionSnapshot.PermissionId)}",
            param: parameters,
            transaction: UnitOfWork.Transaction);
        
        var userSnapshot = result.FirstOrDefault();
        return userSnapshot?.RestoreFromSnapshot();
    }
    
    public async Task<UserEntity?> FindByEmailAsync(string email, CancellationToken token = default)
    {
        var parameters = new { p_email = email };
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = UserSql.Select.FindByEmail;
        
        var userDict = new Dictionary<string, UserSnapshot>();
        
        var result = await connection.QueryAsync<UserSnapshot, AssignedUserRoleSnapshot?, AssignedUserPermissionSnapshot?, UserSnapshot>(
            sql,
            map: (user, role, permission) =>
            {
                if (!userDict.TryGetValue(user.UserId, out _))
                {
                    user.Roles = [];
                    user.Permissions = [];
                    userDict[user.UserId] = user;
                }

                if (role is not null 
                    && !userDict[user.UserId].Roles.Any(
                        r => r.RoleId == role.RoleId && r.NamespaceId == role.NamespaceId))
                {
                    userDict[user.UserId].Roles.Add(role);
                }

                if (permission is not null 
                    && !userDict[user.UserId].Permissions.Any(p =>
                        p.PermissionId == permission.PermissionId && p.NamespaceId == permission.NamespaceId))
                {
                    userDict[user.UserId].Permissions.Add(permission);
                }

                return user;
            },
            splitOn: $"{nameof(AssignedUserRoleSnapshot.RoleId)}, {nameof(AssignedUserPermissionSnapshot.PermissionId)}",
            param: parameters, 
            transaction: UnitOfWork.Transaction);
        
        var userSnapshot = result.FirstOrDefault();
        return userSnapshot?.RestoreFromSnapshot();
    }

    public async Task<List<ExternalRoleMappingValue>> GetExternalRoleMappingsAsync(CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = UserSql.Select.GetExternalRolesMappings;
        
        var result = await connection.QueryAsync<ExternalRoleMappingSnapshot>(
            sql,
            transaction: UnitOfWork.Transaction);

        return result.Select(x => x.RestoreFromSnapshot()).ToList();
    }

    public async Task<UserEntity?> GetOldestByUpdatedAtUserAsync(CancellationToken token = default)
    {
        const string sql = UserSql.Select.GetOldestByUpdatedAtUserAsync;

        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var user = await connection.QueryFirstOrDefaultAsync<UserSnapshot>(
            sql, 
            transaction: UnitOfWork.Transaction
        );

        return user?.RestoreFromSnapshot();
    }

    public async Task UpdateUserRolesAsync(UserEntity user, List<ExternalRoleMappingValue> currentUserRolesFromKeycloak, CancellationToken token)
    {
        var dataTable = new DataTable();
    
        dataTable.Columns.Add(nameof(Public.UsersRolesLink.UserId), typeof(string));
        dataTable.Columns.Add(nameof(Public.UsersRolesLink.RoleId), typeof(int));
        dataTable.Columns.Add(nameof(Public.UsersRolesLink.NamespaceId), typeof(string));
        dataTable.Columns.Add(nameof(Public.UsersRolesLink.IsManual), typeof(bool));
        
        foreach (var roleMapping in currentUserRolesFromKeycloak)
        {
            dataTable.Rows.Add(user.Id, roleMapping.Role.Value, roleMapping.NamespaceId, false);
        }

        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string TempUserRoles = "temp_user_roles";  
        
        await using var cmd = connection.CreateCommand();
        cmd.Transaction = UnitOfWork.Transaction as NpgsqlTransaction;
        cmd.CommandText = $"""
          CREATE TEMPORARY TABLE {TempUserRoles} (
              {nameof(Public.UsersRolesLink.UserId)} TEXT,
              {nameof(Public.UsersRolesLink.RoleId)} INT,
              {nameof(Public.UsersRolesLink.NamespaceId)} TEXT,
              {nameof(Public.UsersRolesLink.IsManual)} BOOLEAN
          );
        """;
        await cmd.ExecuteNonQueryAsync(token);
        
        const string copySql =
            $"""
             COPY {TempUserRoles} (
                 {nameof(Public.UsersRolesLink.UserId)}, 
                 {nameof(Public.UsersRolesLink.RoleId)}, 
                 {nameof(Public.UsersRolesLink.NamespaceId)}, 
                 {nameof(Public.UsersRolesLink.IsManual)}
             ) FROM STDIN (FORMAT BINARY)
             """;

        await using (var writer = await connection.BeginBinaryImportAsync(copySql, token))
        {
            foreach (DataRow row in dataTable.Rows)
            {
                await writer.StartRowAsync(token);
                await writer.WriteAsync(row[nameof(Public.UsersRolesLink.UserId)], NpgsqlTypes.NpgsqlDbType.Text, token);
                await writer.WriteAsync(row[nameof(Public.UsersRolesLink.RoleId)], NpgsqlTypes.NpgsqlDbType.Integer, token);
                await writer.WriteAsync(row[nameof(Public.UsersRolesLink.NamespaceId)], NpgsqlTypes.NpgsqlDbType.Text, token);
                await writer.WriteAsync(row[nameof(Public.UsersRolesLink.IsManual)], NpgsqlTypes.NpgsqlDbType.Boolean, token);
            }

            await writer.CompleteAsync(token);
        }

        await using var mergeCmd = connection.CreateCommand();
        mergeCmd.Transaction = UnitOfWork.Transaction as NpgsqlTransaction;
        mergeCmd.CommandText = $"""
        -- We delete records that have is_manual = false and are no longer needed for this user
        DELETE FROM {Public.UserRoleLinkTable} target
        USING {TempUserRoles} source
        WHERE target.{Public.UsersRolesLink.IsManual} = false
            AND NOT EXISTS (
                SELECT 1
                FROM {TempUserRoles} s
                WHERE s.{nameof(Public.UsersRolesLink.UserId)} = target.{Public.UsersRolesLink.UserId}
                    AND s.{nameof(Public.UsersRolesLink.RoleId)} = target.{Public.UsersRolesLink.RoleId}
                    AND s.{nameof(Public.UsersRolesLink.NamespaceId)} = target.{Public.UsersRolesLink.NamespaceId}
          );
        
        -- If the records match, we do nothing
        MERGE INTO {Public.UserRoleLinkTable} AS target
        USING {TempUserRoles} AS source
        ON target.{Public.UsersRolesLink.UserId} = source.{nameof(Public.UsersRolesLink.UserId)}
            AND target.{Public.UsersRolesLink.RoleId} = source.{nameof(Public.UsersRolesLink.RoleId)}
            AND target.{Public.UsersRolesLink.NamespaceId} = source.{nameof(Public.UsersRolesLink.NamespaceId)}
        WHEN NOT MATCHED THEN
            INSERT (
                {Public.UsersRolesLink.UserId}, 
                {Public.UsersRolesLink.RoleId}, 
                {Public.UsersRolesLink.NamespaceId}, 
                {Public.UsersRolesLink.IsManual}
            )
            VALUES (
                source.{nameof(Public.UsersRolesLink.UserId)}, 
                source.{nameof(Public.UsersRolesLink.RoleId)}, 
                source.{nameof(Public.UsersRolesLink.NamespaceId)},
                false
            );
        """;
        
        await mergeCmd.ExecuteNonQueryAsync(token);
    }

    public async Task AddAsync(UserEntity entity, CancellationToken token = default)
    {
        var userSnapshot = entity.Save();

        const string insertUserSql = UserSql.Insert.AddUser;

        var connection = await Factory.GetOrCreateConnectionAsync(token);

        await connection.ExecuteAsync(insertUserSql, userSnapshot, transaction: UnitOfWork.Transaction);

        const string insertRolesSql = $"""
            INSERT INTO {Public.UserRoleLinkTable} (
                {Public.UsersRolesLink.UserId},
                {Public.UsersRolesLink.RoleId},
                {Public.UsersRolesLink.NamespaceId},
                {Public.UsersRolesLink.IsManual}
            )
            SELECT @UserId, @RoleId, @NamespaceId, @IsManual
            FROM {Public.NamespaceTable}
            WHERE {Public.Namespaces.Id} = @NamespaceId
            ON CONFLICT DO NOTHING;
        """;

        var rolesParams = userSnapshot.Roles.Select(r => new
        {
            UserId = userSnapshot.UserId,
            RoleId = r.RoleId,
            NamespaceId = r.NamespaceId,
            IsManual = r.IsManual
        });

        if (rolesParams.Any())
        {
            await connection.ExecuteAsync(insertRolesSql, rolesParams, transaction: UnitOfWork.Transaction);
        }

        const string insertPermissionsSql = $"""
            INSERT INTO {Public.UserPermissionLinkTable} (
                {Public.UsersPermissionsLink.UserId},
                {Public.UsersPermissionsLink.PermissionId},
                {Public.UsersPermissionsLink.NamespaceId},
                {Public.UsersPermissionsLink.IsManual}
            )
            SELECT @UserId, @PermissionId, @NamespaceId, @IsManual
            FROM {Public.NamespaceTable}
            WHERE {Public.Namespaces.Id} = @NamespaceId
            ON CONFLICT DO NOTHING;
        """;

        var permissionsParams = userSnapshot.Permissions.Select(p => new
        {
            UserId = userSnapshot.UserId,
            PermissionId = p.PermissionId,
            NamespaceId = p.NamespaceId,
            IsManual = p.IsManual
        });

        if (permissionsParams.Any())
        {
            await connection.ExecuteAsync(insertPermissionsSql, permissionsParams, transaction: UnitOfWork.Transaction);
        }
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_id = id.ToString() };

        const string sql = $"""
            DELETE FROM {Public.UserTable} 
            WHERE {Public.Users.Id} = @p_id
        """;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, param: parameters, transaction: UnitOfWork.Transaction);
    }

    public async Task BulkInsertAsync(IList<UserEntity> users, CancellationToken token = default)
    {
        // todo: implement real bulk insert via temp table
        foreach (var user in users)
        {
            await AddAsync(user, token);
        }
    }
}