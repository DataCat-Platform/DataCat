namespace DataCat.Storage.Postgres.Repositories;

public sealed class UserRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork)
    : IDefaultRepository<UserEntity, Guid>
{
    public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { UserId = id.ToString() };
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var sql = $"SELECT * FROM {Public.UserTable} WHERE {Public.Users.UserId} = @UserId";
        var result = await connection.QueryAsync<UserSnapshot>(sql, param: parameters, transaction: UnitOfWork.Transaction);

        var userSnapshot = result.FirstOrDefault();
        return userSnapshot?.RestoreFromSnapshot();
    }

    public async IAsyncEnumerable<UserEntity> SearchAsync(
        string? filter = null, 
        int page = 1, 
        int pageSize = 10, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var offset = (page - 1) * pageSize;
        var sql = $"SELECT * FROM {Public.UserTable} ";
        
        var parameters = new { Filter = $"{filter}%", PageSize = pageSize, Offset = offset };

        if (!string.IsNullOrEmpty(filter))
        {
            sql += $" WHERE {Public.Users.UserName} LIKE @Filter ";
        }

        sql += " LIMIT @PageSize OFFSET @Offset";
        
        await using var reader = await connection.ExecuteReaderAsync(
            sql, 
            parameters,
            transaction: UnitOfWork.Transaction);
        
        while (await reader.ReadAsync(token))
        {
            var snapshot = reader.ReadUser();
            yield return snapshot.RestoreFromSnapshot();
        }
    }

    public async Task AddAsync(UserEntity entity, CancellationToken token = default)
    {
        var userSnapshot = entity.Save();

        var sql = 
            $"""
             INSERT INTO {Public.UserTable} (
                 {Public.Users.UserId},
                 {Public.Users.UserName},
                 {Public.Users.UserEmail},
                 {Public.Users.UserRole}
             )
             VALUES (@UserId, @UserName, @UserEmail, @UserRole)
             """;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, userSnapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task UpdateAsync(UserEntity entity, CancellationToken token = default)
    {
        var userSnapshot = entity.Save();

        var sql = 
            $"""
             UPDATE {Public.UserTable} 
             SET 
                 {Public.Users.UserName} = @UserName, 
                 {Public.Users.UserEmail} = @UserEmail, 
                 {Public.Users.UserRole} = @UserRole
             WHERE {Public.Users.UserId} = @UserId
             """;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, userSnapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { UserId = id.ToString() };

        var sql = $"DELETE FROM {Public.UserTable} WHERE {Public.Users.UserId} = @UserId";
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, param: parameters, transaction: UnitOfWork.Transaction);
    }
}