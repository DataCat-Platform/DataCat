namespace DataCat.Server.Application.Validations;

public class CustomValidationException(IList<ValidationError> failures) : Exception
{
    public IReadOnlyList<ValidationError> Failures { get; } = failures.AsReadOnly();
}