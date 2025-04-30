namespace DataCat.Server.Application.Queries.NotificationDestinations.Get;

public sealed class GetNotificationDestinationQueryHandler(
    IDataSourceTypeRepository dataSourceTypeRepository) 
    : IRequestHandler<GetNotificationDestinationQuery, Result<GetNotificationDestinationResponse>>
{
    public async Task<Result<GetNotificationDestinationResponse>> Handle(GetNotificationDestinationQuery request, CancellationToken cancellationToken)
    {
        var notificationDestination = await dataSourceTypeRepository.GetByNameAsync(request.Name, cancellationToken);
        return notificationDestination is null 
            ? Result.Fail<GetNotificationDestinationResponse>(NotificationDestinationError.NotFound(request.Name)) 
            : Result.Success(new GetNotificationDestinationResponse(notificationDestination.Id, notificationDestination.Name));
    }
}