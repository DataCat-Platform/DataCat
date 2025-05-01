namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetServices;

public sealed class GetServicesQueryValidator : AbstractValidator<GetServicesQuery>
{
    public GetServicesQueryValidator()
    {
        RuleFor(x => x.DataSourceName).NotEmpty();
    }
}