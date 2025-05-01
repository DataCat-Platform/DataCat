namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetTrace;

public sealed class GetTraceQueryValidator : AbstractValidator<GetTraceQuery>
{
    public GetTraceQueryValidator()
    {
        RuleFor(x => x.DataSourceName).NotEmpty();
        RuleFor(x => x.TraceId).NotEmpty();
    }
}