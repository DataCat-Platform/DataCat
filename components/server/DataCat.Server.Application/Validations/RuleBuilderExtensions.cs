namespace DataCat.Server.Application.Validations;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string?> MustBeGuid<T>(this IRuleBuilder<T, string?> ruleBuilder, string? errorMessage = null)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .Must(input => Guid.TryParse(input, out _))
            .WithMessage(errorMessage ?? "Value must be a valid Guid.");
    }
}