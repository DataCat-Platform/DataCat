namespace DataCat.Server.Application.Queries.NotificationDestinations.Get;

public sealed class GetNotificationDestinationQueryValidator : AbstractValidator<GetNotificationDestinationQuery>
{
    public GetNotificationDestinationQueryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}