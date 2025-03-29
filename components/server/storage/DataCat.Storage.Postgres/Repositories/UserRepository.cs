using DataCat.Server.Domain.Common;

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

        const string sql = $"""
            SELECT
                {Public.Users.Id}       {nameof(UserSnapshot.UserId)}
            FROM {Public.UserTable} 
            WHERE {Public.Users.Id} = @p_user_id
        """;
        
        var result = await connection.QueryAsync<UserSnapshot>(sql, param: parameters, transaction: UnitOfWork.Transaction);
        var userSnapshot = result.FirstOrDefault();
        return userSnapshot?.RestoreFromSnapshot();
    }

    public async Task AddAsync(UserEntity entity, CancellationToken token = default)
    {
        var userSnapshot = entity.Save();

        const string sql = $@"
            INSERT INTO {Public.UserTable} (
                {Public.Users.Id}
            )
            VALUES (@UserId)
        ";
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, userSnapshot, transaction: UnitOfWork.Transaction);
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
}