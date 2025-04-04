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