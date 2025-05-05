namespace DataCat.Server.Application.Commands.Dashboards.Remove;

public sealed class RemoveDataSourceValidator : AbstractValidator<RemoveDashboardCommand>
{
    public RemoveDataSourceValidator()
    {
        RuleFor(x => x.DashboardId).NotEmpty().MustBeGuid();
    }
}