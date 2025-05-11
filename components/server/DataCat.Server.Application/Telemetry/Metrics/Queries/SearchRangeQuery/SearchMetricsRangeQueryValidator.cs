namespace DataCat.Server.Application.Telemetry.Metrics.Queries.SearchRangeQuery;

public sealed class SearchMetricsRangeQueryValidator : AbstractValidator<SearchMetricsRangeQuery>
{
    public SearchMetricsRangeQueryValidator()
    {
        RuleFor(x => x.DataSourceName).NotEmpty();
        RuleFor(x => x.Query).NotEmpty();

        RuleFor(x => x.Start)
            .LessThan(x => x.End).WithMessage("Start date must be before End date")
            .GreaterThan(DateTime.MinValue).WithMessage("Invalid Start date");

        RuleFor(x => x.End)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("End date cannot be in the future")
            .GreaterThan(DateTime.MinValue).WithMessage("Invalid End date");

        RuleFor(x => x.Step)
            .GreaterThan(TimeSpan.Zero).WithMessage("Step must be positive");
    }
}