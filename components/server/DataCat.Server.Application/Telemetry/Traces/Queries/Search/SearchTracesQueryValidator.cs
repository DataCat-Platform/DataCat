namespace DataCat.Server.Application.Telemetry.Traces.Queries.Search;

public sealed partial class SearchTracesQueryValidator : AbstractValidator<SearchTracesQuery>
{
    public SearchTracesQueryValidator()
    {
        RuleFor(x => x.DataSourceName).NotEmpty();
        RuleFor(x => x.Service).NotEmpty();
        RuleFor(x => x.Start).NotEmpty().LessThan(x => x.End);
        RuleFor(x => x.End).NotEmpty().GreaterThan(x => x.Start);
        RuleFor(x => x.Limit).GreaterThan(0).When(x => x.Limit.HasValue);
        RuleFor(x => x.MinDuration)
            .Must(BeAValidDuration)
            .When(x => x.MinDuration != null)
            .WithMessage("MinDuration must be a valid duration (e.g., 1.2s, 500ms, 250us).")
            .Must(BeGreaterThanZero)
            .When(x => x.MinDuration != null)
            .WithMessage("MinDuration must be greater than zero.");

        RuleFor(x => x.MaxDuration)
            .Must(BeAValidDuration)
            .When(x => x.MaxDuration != null)
            .WithMessage("MaxDuration must be a valid duration (e.g., 1.2s, 500ms, 250us).")
            .Must(BeGreaterThanZero)
            .When(x => x.MaxDuration != null)
            .WithMessage("MaxDuration must be greater than zero.");
    }
    
    private static bool BeAValidDuration(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var regex = TimeRegex();
        return regex.IsMatch(value);
    }

    private static bool BeGreaterThanZero(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (TimeSpan.TryParse(value, out var timeSpan))
        {
            return timeSpan > TimeSpan.Zero;
        }

        if (TryParseCustomDuration(value, out var customTimeSpan))
        {
            return customTimeSpan > TimeSpan.Zero;
        }

        return false;
    }

    private static bool TryParseCustomDuration(string value, out TimeSpan result)
    {
        result = TimeSpan.Zero;
        var regex = TimeRegex();
        var match = regex.Match(value);

        if (!match.Success)
            return false;

        var number = double.Parse(match.Groups[1].Value);
        var unit = match.Groups[3].Value.ToLower();

        switch (unit)
        {
            case "s":
                result = TimeSpan.FromSeconds(number);
                break;
            case "ms":
                result = TimeSpan.FromMilliseconds(number);
                break;
            case "us":
                result = TimeSpan.FromTicks((long)(number * 10));
                break;
            default:
                return false;
        }

        return true;
    }

    [GeneratedRegex(@"^(\d+(\.\d+)?)\s?(s|ms|us)$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex TimeRegex();
}