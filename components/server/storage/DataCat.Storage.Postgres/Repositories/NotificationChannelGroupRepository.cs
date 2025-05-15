namespace DataCat.Storage.Postgres.Repositories;

public sealed class NotificationChannelGroupRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork,
    NotificationChannelManager NotificationChannelManager,
    NamespaceContext NamespaceContext)
    : IRepository<NotificationChannelGroup, Guid>, INotificationChannelGroupRepository
{
    public async Task<NotificationChannelGroup?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_notification_channel_group_id = id.ToString(), p_namespace_id = NamespaceContext.NamespaceId };

        const string sql = $"""
            SELECT
                notification_group.{Public.NotificationChannelGroups.Id}            {nameof(NotificationChannelGroupSnapshot.Id)},
                notification_group.{Public.NotificationChannelGroups.Name}          {nameof(NotificationChannelGroupSnapshot.Name)},
                notification_group.{Public.NotificationChannelGroups.NamespaceId}   {nameof(NotificationChannelGroupSnapshot.NamespaceId)},
                
                notification_channel.{Public.NotificationChannels.Id}                           {nameof(NotificationChannelSnapshot.Id)},
                notification_channel.{Public.NotificationChannels.NotificationChannelGroupId}   {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                notification_channel.{Public.NotificationChannels.DestinationId}                {nameof(NotificationChannelSnapshot.DestinationId)},
                notification_channel.{Public.NotificationChannels.Settings}                     {nameof(NotificationChannelSnapshot.Settings)},
                notification_channel.{Public.NotificationChannels.NamespaceId}                  {nameof(NotificationChannelSnapshot.NamespaceId)},
                
                notification_destination.{Public.NotificationDestination.Id}                 {nameof(NotificationDestinationSnapshot.Id)},
                notification_destination.{Public.NotificationDestination.Name}               {nameof(NotificationDestinationSnapshot.Name)}
            
            FROM
                {Public.NotificationChannelGroupTable} notification_group
            LEFT JOIN
                {Public.NotificationChannelTable} notification_channel ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
            LEFT JOIN 
                {Public.NotificationDestinationTable} notification_destination ON notification_channel.{Public.NotificationChannels.DestinationId} = notification_destination.{Public.NotificationChannels.Id} 
            WHERE notification_group.{Public.NotificationChannelGroups.Id} = @p_notification_channel_group_id AND notification_group.{Public.NotificationChannels.NamespaceId} = @p_namespace_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        var groupDictionary = new Dictionary<string, NotificationChannelGroupSnapshot>();

        await connection.QueryAsync<
            NotificationChannelGroupSnapshot,
            NotificationChannelSnapshot?,
            NotificationDestinationSnapshot?,
            NotificationChannelGroupSnapshot>(
            sql,
            map: (group, notification, destination) =>
            {
                if (!groupDictionary.TryGetValue(group.Id, out var groupEntry))
                {
                    groupEntry = group;
                    groupDictionary.Add(groupEntry.Id, groupEntry);
                }
                
                if (notification is not null)
                {
                    notification.Destination = destination!;
                    groupEntry.Channels.Add(notification);
                }

                return groupEntry;
            },
            splitOn: $"{nameof(NotificationChannelSnapshot.Id)}, {nameof(NotificationDestinationSnapshot.Id)}",
            param: parameters,
            transaction: unitOfWork.Transaction);

        return groupDictionary.Values.FirstOrDefault()?.RestoreFromSnapshot(NotificationChannelManager);
    }

    public async Task<List<NotificationChannelGroup>> GetAllAsync(CancellationToken token = default)
    {
        var parameters = new { p_namespace_id = NamespaceContext.NamespaceId };
        
        const string sql = $"""
            SELECT
                notification_group.{Public.NotificationChannelGroups.Id}            {nameof(NotificationChannelGroupSnapshot.Id)},
                notification_group.{Public.NotificationChannelGroups.Name}          {nameof(NotificationChannelGroupSnapshot.Name)},
                notification_group.{Public.NotificationChannelGroups.NamespaceId}   {nameof(NotificationChannelGroupSnapshot.NamespaceId)},
                
                notification_channel.{Public.NotificationChannels.Id}                           {nameof(NotificationChannelSnapshot.Id)},
                notification_channel.{Public.NotificationChannels.NotificationChannelGroupId}   {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                notification_channel.{Public.NotificationChannels.DestinationId}                {nameof(NotificationChannelSnapshot.DestinationId)},
                notification_channel.{Public.NotificationChannels.Settings}                     {nameof(NotificationChannelSnapshot.Settings)},
                notification_channel.{Public.NotificationChannels.NamespaceId}                  {nameof(NotificationChannelSnapshot.NamespaceId)},
                
                notification_destination.{Public.NotificationDestination.Id}                {nameof(NotificationDestinationSnapshot.Id)},
                notification_destination.{Public.NotificationDestination.Name}              {nameof(NotificationDestinationSnapshot.Name)}
            
            FROM
                {Public.NotificationChannelGroupTable} notification_group
            LEFT JOIN
                {Public.NotificationChannelTable} notification_channel ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
            LEFT JOIN 
                {Public.NotificationDestinationTable} notification_destination ON notification_channel.{Public.NotificationChannels.DestinationId} = notification_destination.{Public.NotificationChannels.Id}
            WHERE notification_group.{Public.NotificationChannelGroups.NamespaceId} = @p_namespace_id   
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        var groupDictionary = new Dictionary<string, NotificationChannelGroupSnapshot>();

        await connection.QueryAsync<
            NotificationChannelGroupSnapshot,
            NotificationChannelSnapshot?,
            NotificationDestinationSnapshot?,
            NotificationChannelGroupSnapshot>(
            sql,
            map: (group, notification, destination) =>
            {
                if (!groupDictionary.TryGetValue(group.Id, out var groupEntry))
                {
                    groupEntry = group;
                    groupDictionary.Add(groupEntry.Id, groupEntry);
                }

                if (notification is not null)
                {
                    notification.Destination = destination!;
                    groupEntry.Channels.Add(notification);
                }

                return groupEntry;
            },
            param: parameters,
            splitOn: $"{nameof(NotificationChannelSnapshot.Id)}, {nameof(NotificationDestinationSnapshot.Id)}",
            transaction: unitOfWork.Transaction);

        return groupDictionary.Values.Select(x => x.RestoreFromSnapshot(NotificationChannelManager)).ToList();
    }

    public async Task<Page<NotificationChannelGroup>> SearchAsync(SearchFilters filters, int page = 1, int pageSize = 10, CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var parameters = new DynamicParameters();

        var offset = (page - 1) * pageSize;
        parameters.Add("p_namespace_id", NamespaceContext.NamespaceId);
        parameters.Add("offset", offset);
        parameters.Add("limit", pageSize);

        var columnMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = $"notification_group.{Public.NotificationChannelGroups.Id}",
            ["name"] = $"notification_group.{Public.NotificationChannelGroups.Name}"
        };
        
        var countSql = new StringBuilder();
        countSql.AppendLine($"""
            SELECT COUNT(*)
            FROM {Public.NotificationChannelGroupTable} notification_group
            LEFT JOIN {Public.NotificationChannelTable} notification_channel
                ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
            WHERE notification_group.{Public.NotificationChannelGroups.NamespaceId} = @p_namespace_id 
        """);
        
        countSql.BuildQuery(parameters, filters, columnMappings);
        
        var countSqlString = countSql.ToString();
        
        var totalCount = await connection.QuerySingleAsync<int>(
            countSqlString,
            parameters,
            transaction: unitOfWork.Transaction);
        
        var dataSql = new StringBuilder();
        dataSql.AppendLine($"""
                SELECT
                    notification_group.{Public.NotificationChannelGroups.Id}             {nameof(NotificationChannelGroupSnapshot.Id)},
                    notification_group.{Public.NotificationChannelGroups.Name}           {nameof(NotificationChannelGroupSnapshot.Name)},
                    notification_group.{Public.NotificationChannelGroups.NamespaceId}    {nameof(NotificationChannelGroupSnapshot.NamespaceId)},
                    
                    notification_channel.{Public.NotificationChannels.Id}                             {nameof(NotificationChannelSnapshot.Id)},
                    notification_channel.{Public.NotificationChannels.NotificationChannelGroupId}     {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                    notification_channel.{Public.NotificationChannels.DestinationId}                  {nameof(NotificationChannelSnapshot.DestinationId)},
                    notification_channel.{Public.NotificationChannels.Settings}                       {nameof(NotificationChannelSnapshot.Settings)},
                    notification_channel.{Public.NotificationChannels.NamespaceId}                    {nameof(NotificationChannelSnapshot.NamespaceId)},
                    
                    notification_destination.{Public.NotificationDestination.Id}        {nameof(NotificationDestinationSnapshot.Id)},
                    notification_destination.{Public.NotificationDestination.Name}      {nameof(NotificationDestinationSnapshot.Name)}
                FROM 
                    {Public.NotificationChannelGroupTable} notification_group
                LEFT JOIN 
                    {Public.NotificationChannelTable} notification_channel ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
                LEFT JOIN 
                    {Public.NotificationDestinationTable} notification_destination ON notification_channel.{Public.NotificationChannels.DestinationId} = notification_destination.{Public.NotificationDestination.Id}
                WHERE notification_group.{Public.NotificationChannelGroups.NamespaceId} = @p_namespace_id 
        """);
        
        dataSql
            .BuildQuery(parameters, filters, columnMappings)
            .ApplyOrderBy(filters.Sort ?? new Sort(FieldName: $"notification_group.{Public.NotificationChannelGroups.Id}"), columnMappings)
            .ApplyPagination();
        
        var dataSqlString = dataSql.ToString();
        
        var groupDictionary = new Dictionary<string, NotificationChannelGroupSnapshot>();

        var result = await connection.QueryAsync<
            NotificationChannelGroupSnapshot,
            NotificationChannelSnapshot?,
            NotificationDestinationSnapshot?,
            NotificationChannelGroupSnapshot>(
            dataSqlString,
            map: (group, notification, destination) =>
            {
                if (!groupDictionary.TryGetValue(group.Id, out var groupEntry))
                {
                    groupEntry = group;
                    groupDictionary.Add(groupEntry.Id, groupEntry);
                }

                if (notification is not null)
                {
                    notification.Destination = destination!;
                    groupEntry.Channels.Add(notification);    
                }

                return groupEntry;
            },
            splitOn: $"{nameof(NotificationChannelSnapshot.Id)}, {nameof(NotificationDestinationSnapshot.Id)}",
            param: parameters,
            transaction: unitOfWork.Transaction);

        var items = result.Select(x => x.RestoreFromSnapshot(NotificationChannelManager));
        return new Page<NotificationChannelGroup>(items, totalCount, page, pageSize);
    }

    public async Task<NotificationChannelGroup?> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var parameters = new { p_notification_channel_group_name = name, p_namespace_id = NamespaceContext.NamespaceId };

        const string sql = $"""
            SELECT
                notification_group.{Public.NotificationChannelGroups.Id}           {nameof(NotificationChannelGroupSnapshot.Id)},
                notification_group.{Public.NotificationChannelGroups.Name}         {nameof(NotificationChannelGroupSnapshot.Name)},
                notification_group.{Public.NotificationChannelGroups.NamespaceId}  {nameof(NotificationChannelGroupSnapshot.NamespaceId)},
                
                notification_channel.{Public.NotificationChannels.Id}                           {nameof(NotificationChannelSnapshot.Id)},
                notification_channel.{Public.NotificationChannels.NotificationChannelGroupId}   {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                notification_channel.{Public.NotificationChannels.DestinationId}                {nameof(NotificationChannelSnapshot.DestinationId)},
                notification_channel.{Public.NotificationChannels.Settings}                     {nameof(NotificationChannelSnapshot.Settings)},
                notification_channel.{Public.NotificationChannels.NamespaceId}                  {nameof(NotificationChannelSnapshot.NamespaceId)},
                
                notification_destination.{Public.NotificationDestination.Id}        {nameof(NotificationDestinationSnapshot.Id)},
                notification_destination.{Public.NotificationDestination.Name}      {nameof(NotificationDestinationSnapshot.Name)}
            
            FROM
                {Public.NotificationChannelGroupTable} notification_group
            LEFT JOIN
                {Public.NotificationChannelTable} notification_channel ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
            LEFT JOIN 
                {Public.NotificationDestinationTable} notification_destination ON notification_channel.{Public.NotificationChannels.DestinationId} = notification_destination.{Public.NotificationDestination.Id} 
            WHERE notification_group.{Public.NotificationChannelGroups.Name} = @p_notification_channel_group_name AND notification_group.{Public.NotificationChannelGroups.NamespaceId} = @p_namespace_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(cancellationToken);
        
        var groupDictionary = new Dictionary<string, NotificationChannelGroupSnapshot>();

        await connection.QueryAsync<
            NotificationChannelGroupSnapshot,
            NotificationChannelSnapshot?,
            NotificationDestinationSnapshot?,
            NotificationChannelGroupSnapshot>(
            sql,
            map: (group, notification, destination) =>
            {
                if (!groupDictionary.TryGetValue(group.Id, out var groupEntry))
                {
                    groupEntry = group;
                    groupDictionary.Add(groupEntry.Id, groupEntry);
                }

                if (notification is not null)
                {
                    notification.Destination = destination!;
                    groupEntry.Channels.Add(notification);    
                }

                return groupEntry;
            },
            splitOn: $"{nameof(NotificationChannelSnapshot.Id)}, {nameof(NotificationDestinationSnapshot.Id)}",
            param: parameters,
            transaction: unitOfWork.Transaction);

        return groupDictionary.Values.FirstOrDefault()?.RestoreFromSnapshot(NotificationChannelManager);
    }

    public async Task AddAsync(NotificationChannelGroup entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();

        const string sql = $@"
            INSERT INTO {Public.NotificationChannelGroupTable} (
                {Public.NotificationChannelGroups.Id},
                {Public.NotificationChannelGroups.Name},
                {Public.NotificationChannelGroups.NamespaceId}
            )
            VALUES (
                @{nameof(NotificationChannelGroupSnapshot.Id)},
                @{nameof(NotificationChannelGroupSnapshot.Name)},
                @{nameof(NotificationChannelGroupSnapshot.NamespaceId)}
            )";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: unitOfWork.Transaction);
    }
}