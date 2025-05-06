namespace DataCat.Server.Application.Commands.DataSources.UpdateConnectionString;

public sealed class UpdateConnectionStringDataSourceCommandValidator : AbstractValidator<UpdateConnectionStringDataSourceCommand>
{
    public UpdateConnectionStringDataSourceCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.ConnectionString).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}