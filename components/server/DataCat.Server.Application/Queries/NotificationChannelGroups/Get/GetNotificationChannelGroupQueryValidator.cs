namespace DataCat.Server.Application.Queries.NotificationChannelGroups.Get;

public sealed class GetNotificationChannelGroupQueryValidator : AbstractValidator<GetNotificationChannelGroupQuery>
{
    public GetNotificationChannelGroupQueryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}