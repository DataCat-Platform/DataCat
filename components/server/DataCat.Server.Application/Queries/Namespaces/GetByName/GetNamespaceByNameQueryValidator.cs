namespace DataCat.Server.Application.Queries.Namespaces.GetByName;

public sealed class GetNamespaceByNameQueryValidator : AbstractValidator<GetNamespaceByNameQuery>
{
    public GetNamespaceByNameQueryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}