namespace DataCat.Server.Application.Telemetry.Metrics.Queries.SearchQuery;

public sealed class SearchMetricsQueryValidator : AbstractValidator<SearchMetricsQuery>
{
    public SearchMetricsQueryValidator()
    {
        RuleFor(x => x.DataSourceName).NotEmpty();
        RuleFor(x => x.Query).NotEmpty();
    }
}