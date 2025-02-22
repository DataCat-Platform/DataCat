namespace DataCat.Storage.Postgres.Repositories;

public sealed class DashboardAccessRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory)
    : IDashboardAccessRepository
{
    public async Task AddUserToDashboard(UserEntity user, DashboardEntity dashboard, CancellationToken token = default)
    {
        var parameters = new
        {
            dashboard_id = dashboard.Id.ToString(),
            user_id = user.Id.ToString(),
        };
        var sql = 
            $"""
             INSERT INTO {ManyToManyRelationShips.DashboardUserLinkTable} (
                {Public.Dashboards.DashboardId},
                {Public.Users.UserId}
             ) 
             VALUES (
                @dashboard_id,
                @user_id
             );
             """;
        
        var command = new CommandDefinition(sql, parameters);
        var connection = await Factory.CreateConnectionAsync(token);

        await connection.ExecuteAsync(command);
    }
}