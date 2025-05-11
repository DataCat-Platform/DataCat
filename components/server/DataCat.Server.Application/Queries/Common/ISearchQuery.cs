namespace DataCat.Server.Application.Queries.Common;

public interface ISearchQuery : IPaginationQuery
{
    SearchFilters Filters { get; }
}

public sealed class SearchQueryValidator : AbstractValidator<ISearchQuery>
{
    public SearchQueryValidator()
    {
        RuleLevelCascadeMode = RuleLevelCascadeMode = CascadeMode.Continue;

        RuleFor(x => x.Filters).NotNull();
        
        RuleFor(x => x.Filters.Sort!.FieldName)
            .NotEmpty()
            .WithMessage("Sort.FieldName must not be empty when Sort is specified.")
            .When(x => x.Filters.Sort is not null);

        RuleForEach(x => x.Filters.Filters).NotNull().ChildRules(filter =>
        {
            filter.RuleFor(x => x.Key)
                .NotEmpty();

            filter.RuleFor(x => x.Value)
                .NotEmpty();

            filter.RuleFor(x => x.MatchMode)
                .Must((input, matchMode) =>
                {
                    return input.FieldType switch
                    {
                        SearchFieldType.String => matchMode is MatchMode.Equals or MatchMode.StartsWith or MatchMode.Contains,
                        SearchFieldType.Number => matchMode == MatchMode.Equals,
                        SearchFieldType.Boolean => matchMode == MatchMode.Equals,
                        SearchFieldType.Array => matchMode == MatchMode.Contains,
                        SearchFieldType.Date => matchMode == MatchMode.Equals,
                        _ => false
                    };
                })
                .WithMessage(input => $"Incorrect MatchMode '{input.MatchMode}' for FieldType '{input.FieldType}'");
        });
        
        Include(new PaginationQueryValidator());
    }
}