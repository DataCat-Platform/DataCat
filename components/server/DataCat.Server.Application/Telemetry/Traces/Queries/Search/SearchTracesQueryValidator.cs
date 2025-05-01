namespace DataCat.Server.Application.Telemetry.Traces.Queries.Search;

public sealed class SearchTracesQueryValidator : AbstractValidator<SearchTracesQuery>
{
    public SearchTracesQueryValidator()
    {
        RuleFor(x => x.DataSourceName).NotEmpty();
        RuleFor(x => x.Service).NotEmpty();
        RuleFor(x => x.Start).NotEmpty().LessThan(x => x.End);
        RuleFor(x => x.End).NotEmpty().GreaterThan(x => x.Start);
        RuleFor(x => x.Limit).GreaterThan(0).When(x => x.Limit.HasValue);
        RuleFor(x => x.MinDuration).GreaterThan(TimeSpan.Zero).When(x => x.MinDuration.HasValue);
        RuleFor(x => x.MaxDuration).GreaterThan(TimeSpan.Zero).When(x => x.MaxDuration.HasValue);
    }
}