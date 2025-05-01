namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetOperations;

public sealed class GetOperationsQueryValidator : AbstractValidator<GetOperationsQuery>
{
    public GetOperationsQueryValidator()
    {
        RuleFor(x => x.DataSourceName).NotEmpty();
        RuleFor(x => x.ServiceName).NotEmpty();
    }
}